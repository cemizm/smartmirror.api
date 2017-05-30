using System;
using System.Threading.Tasks;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer
{
    public interface ITicketRepository
    {
		Task<Ticket> GetTicket(string number);
		Task<Ticket> GetTicketByMirrorId(Guid id);
        Task Add(Ticket ticket);
        Task Delete(string number);
    }
}
