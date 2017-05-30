using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.DataLayer.Models;
using WebApi.DataLayer.MongoDB.Data;

namespace WebApi.DataLayer.MongoDB
{
    public class TicketRepository : ITicketRepository
    {
        private SmartMirrorContext context;

        public TicketRepository(IOptions<MongoSettings> settings)
        {
            context = new SmartMirrorContext(settings.Value);
        }

        public async Task Add(Ticket ticket)
        {
            await context.Tickets.InsertOneAsync(ticket);
        }

        public async Task Delete(string number)
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t.Number, number);
            await context.Tickets.DeleteOneAsync(filter);
        }

        public async Task<Ticket> GetTicket(string number)
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t.Number, number);
            return await context.Tickets.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Ticket> GetTicketByMirrorId(Guid id)
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t.MirrorId, id);
            return await context.Tickets.Find(filter).FirstOrDefaultAsync();
        }
    }
}
