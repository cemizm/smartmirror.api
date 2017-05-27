using System;
using System.Collections.Generic;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetByEmail(string email);
        User GetByEmailPassword(string email, string password);
        void Add(User user);
        void Update(User user);
        void Delete(string email);
    }
}
