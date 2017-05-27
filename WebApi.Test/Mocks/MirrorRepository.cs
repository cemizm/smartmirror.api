using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.DataLayer;
using WebApi.DataLayer.Models;

namespace WebApi.Test.Mocks
{
    public class MirrorRepository : IMirrorRepository
    {
        private Dictionary<string, List<Mirror>> db;

        public static Guid TestId = new Guid("{38f5a067-9095-4c1b-82f8-7a3e6bc3a5a6}");

        public MirrorRepository(){
			db = new Dictionary<string, List<Mirror>>();
			db.Add("a@smart.mirror", new List<Mirror>());
			db.Add("b@smart.mirror", new List<Mirror>());

            db["a@smart.mirror"].Add(new Mirror() { Id = TestId, Name = "TestName", Widgets = new List<Widget>() });
			db["a@smart.mirror"].Add(new Mirror() { Id = new Guid(), Name = "", Widgets = new List<Widget>() });
			db["a@smart.mirror"].Add(new Mirror() { Id = new Guid(), Name = "", Widgets = new List<Widget>() });


            db["b@smart.mirror"].Add(new Mirror() { Id = new Guid(), Name = "", Widgets = new List<Widget>() });
        }

        public void Add(string user, Mirror mirror)
        {
            List<Mirror> mirrors = new List<Mirror>();
            if (!db.TryGetValue(user, out mirrors))
                db.Add(user, mirrors);

            mirrors.Add(mirror);
        }

        public void Delete(string user, Guid id)
        {
            Mirror m = GetById(user, id);

            if (m == null)
                return;

            db[user].Remove(m);
        }

        public IEnumerable<Mirror> GetAll(string user)
		{
			List<Mirror> mirrors = new List<Mirror>();

            db.TryGetValue(user, out mirrors);

            return mirrors;
        }

        public Mirror GetById(string user, Guid id)
		{
			if (!db.ContainsKey(user))
				return null;

			return  db[user].FirstOrDefault(mirror => mirror.Id == id);
        }

        public void Update(string user, Mirror mirror)
		{
			Mirror m = GetById(user, mirror.Id);

            if (m == null)
                return;

            db[user].Remove(m);
            db[user].Add(m);
        }
    }
}
