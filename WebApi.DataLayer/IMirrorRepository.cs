using System;
using System.Collections.Generic;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer
{
    public interface IMirrorRepository
    {
        IEnumerable<Mirror> GetAll(string user);
        Mirror GetById(string user, Guid id);
        void Update(string user, Mirror mirror);
        void Add(string user, Mirror mirror);
        void Delete(string user, Guid id);
    }
}
