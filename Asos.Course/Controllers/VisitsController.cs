using System;
using System.Linq;
using Asos.Course.Models;

namespace Asos.Course.Controllers
{
	public class VisitsController : AbstractController
	{
		public object Create()
		{
			var cities = new[] { "London", "Paris", "Bath", "Berlin", "New York", "San Fransico", "Tijuhana", "Washington", "Pitsborough" };

			var rand = new Random(234);
			foreach (var city in cities)
			{
				var count = rand.Next(10);
				for (int i = 0; i < count; i++)
				{
					Session.Store(new Visit
					{
						At = DateTime.Today,
						Location = city,
						Visitor = "users/" + (i + 1)
					});
				}
			}

			return Json("Done");
		}

		public object Search()
		{
			var j = Session.Advanced.LuceneQuery<Users_VisitedLocations.Result, Users_VisitedLocations>()
				.WhereEquals("Paris_2013", 8)
				.AndAlso()
				.WhereEquals(x => x.Visitor, "users/1")
				.ToArray();

			return Json(j);
		}
	}
}