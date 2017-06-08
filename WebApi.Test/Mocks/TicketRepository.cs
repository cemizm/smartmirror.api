using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;

namespace WebApi.Test.Mocks
{
    public class TicketRepository : ITicketRepository
    {
        public static string TestNumber = "AB5I8L";
        public static string TestDupNumber = "EXISTS";
        public static Guid TestMirrorId = new Guid("{9140587d-29f8-4f33-b194-c953b62b1f7d}");

        private List<Ticket> db;

        public TicketRepository()
        {
            db = new List<Ticket>();
            db.Add(new Ticket() { MirrorId = TestMirrorId, Number = TestNumber });
            db.Add(new Ticket() { MirrorId = MirrorRepository.TestId, Number = TestDupNumber });
            db.Add(new Ticket() { MirrorId = Guid.NewGuid(), Number = "52AS20" });
            db.Add(new Ticket() { MirrorId = Guid.NewGuid(), Number = "87HG65" });
            db.Add(new Ticket() { MirrorId = Guid.NewGuid(), Number = "98UIS2" });
        }

        public Task Add(Ticket ticket)
        {
            return Task.Run(() => { db.Add(ticket); });
        }

        public Task Delete(string number)
        {
            return Task.Run(() =>
            {
                var ticket = db.FirstOrDefault(t => t.Number == number);
                if (ticket == null)
                    return;
                db.Remove(ticket);
            });
        }

        public Task<Ticket> GetTicket(string number)
        {
            return Task.Run(() =>
            {
                return db.FirstOrDefault(t => t.Number == number);
            });
        }

        public Task<Ticket> GetTicketByMirrorId(Guid id)
        {
            return Task.Run(() =>
            {
                return db.FirstOrDefault(t => t.MirrorId == id);
            });
        }
    }
}
