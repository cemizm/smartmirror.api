using System;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer.MongoDB.Data
{
    internal class SmartMirrorContext
    {
        private readonly IMongoDatabase db = null;

        static SmartMirrorContext()
		{
			BsonClassMap.RegisterClassMap<User>(cm =>
			{
				cm.AutoMap();
				cm.MapIdProperty(u => u.Email);
			});
			BsonClassMap.RegisterClassMap<Ticket>(cm =>
			{
				cm.AutoMap();
                cm.MapIdProperty(t => t.Number);
			});
			BsonClassMap.RegisterClassMap<Mirror>(cm =>
			{
				cm.AutoMap();
                cm.MapIdProperty(m => m.Id);
			});
        }

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
