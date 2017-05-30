using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer.MongoDB
{
    public class UserRepository : IUserRepository
    {
        public async Task Add(User user)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmailPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
