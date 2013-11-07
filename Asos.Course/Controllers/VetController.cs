using System.Collections.Generic;
using System.Linq;
using Asos.Course.Models;

namespace Asos.Course.Controllers
{
	public class VetController : AbstractController
	{
		public object Create()
		{
			Session.Store(new Dog
			{
				Name = "Arava",
				Owners = new[]{"people/1", "people/2"}
			});
			Session.Store(new Person
			{
				Name = "Oren"
			});
			Session.Store(new Person
			{
				Name = "Rachel"
			});

			return Json("done");
		}

		public object Load(int id)
		{
			var dog = Session.Load<Dog>(id);

			var owners = new List<string>();
			foreach (var owner in dog.Owners)
			{
				owners.Add(Session.Load<Person>(owner).Name);
			}

			return Json(new
			{
				dog.Name,
				Owners = owners
			});
		}

		public object Transform(int id)
		{
			var dog = Session.Load<DogsAndOwnersTransformer, DogsAndOwnersTransformer.Result>("dogs/" + id);

			Session.Query<Dog>()
				.Select(d => new{ d.Name})
				.ToList();

			return Json(dog);
		}


		public object Include(int id)
		{
			var dog = Session
				.Include<Dog>(x=>x.Owners)
				.Load(id);

			var owners = new List<string>();
			foreach (var owner in dog.Owners)
			{
				owners.Add(Session.Load<Person>(owner).Name);
			}

			return Json(new
			{
				dog.Name,
				Owners = owners
			});
		}
	}
}