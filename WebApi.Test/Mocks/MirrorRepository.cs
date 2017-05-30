using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;

namespace WebApi.Test.Mocks
{
    public class MirrorRepository : IMirrorRepository
    {
        private List<Mirror> db;

        public static Guid TestId = new Guid("{38f5a067-9095-4c1b-82f8-7a3e6bc3a5a6}");

        public MirrorRepository()
        {
            db = new List<Mirror>();

            db.Add(new Mirror() { Id = TestId, User = "a@smart.mirror", Name = "TestName", Widgets = new List<Widget>() });
            db.Add(new Mirror() { Id = Guid.NewGuid(), User = "a@smart.mirror", Name = "", Widgets = new List<Widget>() });
            db.Add(new Mirror() { Id = Guid.NewGuid(), User = "a@smart.mirror", Name = "", Widgets = new List<Widget>() });

            db.Add(new Mirror() { Id = Guid.NewGuid(), User = "b@smart.mirror", Name = "", Widgets = new List<Widget>() });
        }

        public Task Add(Mirror mirror)
        {
            return Task.Run(() => db.Add(mirror));
        }

        public Task Delete(Guid id)
        {
            return Task.Run(() =>
            {
                var mirror = db.FirstOrDefault(m => m.Id == id);
                if (mirror == null)
                    return;
                db.Remove(mirror);
            });
        }

        public Task<IEnumerable<Mirror>> GetAll(string user)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(user))
                    return new List<Mirror>();
                
                return (IEnumerable<Mirror>)db.FindAll(m => string.Compare(m.User, user) == 0);
            });
        }

        public Task<Mirror> GetById(Guid id)
        {
            return Task.Run(() => db.FirstOrDefault(m => m.Id == id));

        }

        public Task Update(Mirror mirror)
        {
            return Delete(mirror.Id).ContinueWith(t => Add(mirror));
        }

    }
}
