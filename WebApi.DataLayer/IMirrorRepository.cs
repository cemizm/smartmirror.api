using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer
{
    public interface IMirrorRepository
    {
        Task<IEnumerable<Mirror>> GetAll(string user);
        Task<Mirror> GetById(Guid id);
        Task Update(Mirror mirror);
        Task Add(Mirror mirror);
        Task Delete(Guid id);
    }
}
