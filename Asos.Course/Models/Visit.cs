using System;
using System.Collections.Generic;
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
		public class LocationYearlyVisit
		{
			public int Year;
			public int Count;
			public string Location;
		}
		public class Result
		{
			public string Visitor { get; set; }
			public LocationYearlyVisit[] Visitations { get; set; }
		}

		public Users_VisitedLocations()
		{
			Map = visits =>
				from visit in visits
				select new
				{
					visit.Visitor,
					Visitations = new[]
					{
						new { visit.At.Year, visit.Location, Count = 1 }
					},
					_ = 1
				};

			Reduce = results =>
				from result in results
				group result by new { result.Visitor }
					into g
					select new
						{
							g.Key.Visitor,
							Visitations = from v in g.SelectMany(x => x.Visitations)
										  group v by new { v.Year, v.Location } into g2
										  select new
										  {
											  g2.Key.Year,
											  g2.Key.Location,
											  Count = g2.Sum(x => x.Count)
										  },
							_ = from v in g.SelectMany(x => x.Visitations)
								group v by new { v.Year, v.Location } into g2
								select CreateField(g2.Key.Location + "_" + g2.Key.Year, g2.Sum(x => x.Count))
						};
		}
	} 
}