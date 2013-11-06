using System;
using System.Linq;
using Asos.Course.Models;
using Raven.Client;

namespace Asos.Course.Controllers
{
	public class HomeController : AbstractController
	{
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