using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.DataLayer.Models;
using WebApi.DataLayer.MongoDB.Data;

namespace WebApi.DataLayer.MongoDB
{
    public class MirrorRepository : IMirrorRepository
    {
        private SmartMirrorContext context;

        public MirrorRepository(IOptions<MongoSettings> settings)
        {
            context = new SmartMirrorContext(settings.Value);
        }

        public async Task Add(Mirror mirror)
        {
            await context.Mirrors.InsertOneAsync(mirror);
        }

        public async Task Delete(Guid id)
        {
            var filter = Builders<Mirror>.Filter.Eq(m => m.Id, id);

            await context.Mirrors.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Mirror>> GetAll(string user = null)
        {
            return await context.Mirrors.Find(m => (string.IsNullOrEmpty(user) ||
                                                    string.Compare(m.User, user, StringComparison.CurrentCultureIgnoreCase) == 0)).ToListAsync();
        }

        public async Task<Mirror> GetById(Guid id)
		{
            return await context.Mirrors.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(Mirror mirror)
        {
            await context.Mirrors.ReplaceOneAsync(m => m.Id == mirror.Id, mirror);
        }
    }
}
