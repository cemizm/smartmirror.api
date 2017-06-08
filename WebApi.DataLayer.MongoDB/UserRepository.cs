using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.DataLayer.Models;
using WebApi.DataLayer.MongoDB.Data;

namespace WebApi.DataLayer.MongoDB
{
    public class UserRepository : IUserRepository
    {
        private SmartMirrorContext context;

        public UserRepository(IOptions<MongoSettings> settings)
        {
            context = new SmartMirrorContext(settings.Value);
        }

        public async Task Add(User user)
        {
            user.Email = user.Email.ToLower();
            user.Name = user.Name.ToLower();

            await context.Users.InsertOneAsync(user);
        }

        public async Task Delete(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email.ToLower());
            await context.Users.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.Find(u => true).ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email.ToLower());
            return await context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailPassword(string email, string password)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email.ToLower()) & Builders<User>.Filter.Eq(u => u.Password, password.ToLower());
            return await context.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Update(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email.ToLower());
            await context.Users.FindOneAndReplaceAsync(filter, user);
        }
    }
}
