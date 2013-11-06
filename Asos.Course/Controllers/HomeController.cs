using System;
using System.Linq;
using System.Security.AccessControl;
using Asos.Course.Indexes;
using Asos.Course.Models;
using		ET.FakeText;
using Raven.Client;

namespace Asos.Course.Controllers
{
	public class HomeController : AbstractController
	{
		public object ProductSearch(string key, string val)
		{
			var queryable = Session.Query<Product, Products_Search>()
				.Where(product => product.Properties[key] == val);
			var y = queryable
				.ToList();
			return Json(y);
		}

		public object Search(string q)
		{
			var results = Session.Query<People_Search.Result, People_Search>()
				.Search(x => x.Query, q)
				.As<Person>()
				.ToList();

			return Json(results);
		}

		public object NewCar()
		{
			Session.Store(new Car
			{
				Color = "Black",
				Make = "Mercedes",
				Model = "C65",
				Milage = 123,
				OnRoadDate = DateTime.Today,
				Type = "Coupe",
				OwnerId = "owners/1"
			});

			return Json("done");
		}

		public object ListCars()
		{
			return Json(Session.Query<Car>().ToList());
		}

		public object LotsOfData(int count)
		{
			var textGenerator = new TextGenerator(WordTypes.Name);
			using (var bulk = DocumentStore.BulkInsert())
			{
				for (int i = 0; i < count; i++)
				{
					bulk.Store(new Person
					{
						Name = textGenerator.GenerateText(i % 2 == 0 ? 2 : 3),
						Email = textGenerator.GenerateWord(i%10+1) + "@" + textGenerator.GenerateWord(i%7+2) + ".com"
					});
				}
			}
			return Json("Done with " + count);
		}

		public object CarsByOwner(string owner)
		{
			var cars = Session.Query<Car>()
				.Include(x => x.OwnerId)
				.Where(x => x.OwnerId == owner)
				.ToList();

			return Json(new
			{
				Owner = Session.Load<Person>(owner),
				Cars = cars,
			});
		}
	}
}