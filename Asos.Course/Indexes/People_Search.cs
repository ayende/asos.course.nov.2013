using System.Linq;
using Asos.Course.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Asos.Course.Indexes
{
	public class People_Search : AbstractIndexCreationTask<Person, People_Search.Result>
	{
		public class Result
		{
			public string Query;
		}

		public People_Search()
		{
			Map = people =>
				from person in people
				select new
				{
					Query = new object[]
					{
						person.Name,
						person.Email,
						person.Email.Split('@')
					}
				};

			Index(x=>x.Query, FieldIndexing.Analyzed);
		}
	}
}