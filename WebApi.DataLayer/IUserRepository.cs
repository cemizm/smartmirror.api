using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataLayer.Models;

namespace WebApi.DataLayer
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> GetByEmailPassword(string email, string password);
        Task Add(User user);
        Task Update(User user);
        Task Delete(string email);
    }
}
