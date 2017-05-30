using System;
using MongoDB.Driver;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer.MongoDB.Data
{
    internal class SmartMirrorContext
    {
        private readonly IMongoDatabase db = null;

        public SmartMirrorContext(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            db = client.GetDatabase(settings.Database);
        }

		public IMongoCollection<User> Users => db.GetCollection<User>("User");

		public IMongoCollection<Ticket> Tickets => db.GetCollection<Ticket>("Ticket");
		
        public IMongoCollection<Mirror> Mirrors => db.GetCollection<Mirror>("Mirror");

    }
}
