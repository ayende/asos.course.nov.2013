using System;
using System.Linq;
using Raven.Client.Indexes;

namespace Asos.Course.Models
{
	public class Visit
	{
		public string Visitor { get; set; } 
		public string Location { get; set; }
		public DateTime At { get; set; }
	}

	public class Users_VisitedLocations : AbstractIndexCreationTask<Visit, Users_VisitedLocations.Result>
	{
		public class Result
		{
			public string Visitor { get; set; }
			public string[] LocationsVisited { get; set; }
		}

		public Users_VisitedLocations()
		{
			Map = visits =>
				from visit in visits
				select new
				{
					visit.Visitor,
					LocationsVisited = new[] {visit.Location}
				};

			Reduce = results =>
				from result in results
				group result by result.Visitor
				into g
				select new
				{
					Visitor = g.Key,
					LocationsVisited = g.SelectMany(x=>x.LocationsVisited).Distinct()
				};
		}
	}
}