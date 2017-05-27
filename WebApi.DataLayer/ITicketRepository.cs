using System;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer
{
    public interface ITicketRepository
    {
        Ticket GetTicket(string number);
        void Add(Ticket ticket);
        void Delete(string number);
    }
}
