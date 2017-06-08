using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;

namespace WebApi.Test.Mocks
{
    public class UserRepository : IUserRepository
    {
        public static string UserId = "a@smart.mirror";
        public static string TestPw = "testpw";
        public static string TestName = "Test Name User A";

        private List<User> users;

        public UserRepository()
        {
            users = new List<User>();
            users.Add(new User() { Email = UserId, Name = TestName, Password = TestPw });
            users.Add(new User() { Email = "b@smart.mirror", Name = "User A", Password = "testpw" });
            users.Add(new User() { Email = "c@smart.mirror", Name = "User c", Password = "testpwc" });
        }

        public Task Add(User user)
        {
            return Task.Run(() => users.Add(user));
        }

        public Task Delete(string email)
        {
            return Task.Run(() =>
            {
                User u = users.Where(a => a.Email == email.ToLower()).FirstOrDefault();
                if (u == null)
                    return;

                users.Remove(u);
            });
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult<IEnumerable<User>>(users);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.Run(() => users.Where(u => u.Email == email.ToLower()).FirstOrDefault());
        }

        public Task<User> GetByEmailPassword(string email, string password)
        {
            return Task.Run(()=>{
                return users.Where(u => u.Email == email.ToLower() && u.Password == password.ToLower()).FirstOrDefault();
            });
        }

        public Task Update(User user)
        {
            return Task.Run(() =>
            {

                User u = users.Where(a => a.Email == user.Email).FirstOrDefault();
                if (u == null)
                    return;

                users.Remove(u);
                users.Add(user);
            });
        }
    }
}
