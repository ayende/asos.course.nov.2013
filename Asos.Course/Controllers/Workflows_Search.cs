using System.Linq;
using Asos.Course.Models;
using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;

namespace Asos.Course.Controllers
{
	public class Workflows_Search : AbstractIndexCreationTask<Workflow, Workflows_Search.Result>
	{
		public class Result
		{
			public string Query;
			public double DaysInProduction;
			public string Brand;
		}

		public Workflows_Search()
		{
			Map = workflows => from wf in workflows
							   select new
							   {
								   wf.DaysInProduction,
								   wf.Brand,
								   Query = new[] { wf.Name, wf.Brand }
							   };


			Sort(x => x.DaysInProduction, SortOptions.Double);
			Index(x => x.Query, FieldIndexing.Analyzed);
			Index(x => x.Brand, FieldIndexing.NotAnalyzed);
			Suggestion(x => x.Query, new SuggestionOptions
			{
				Accuracy = 0.5f,
				Distance = StringDistanceTypes.Default
			});
			TermVector(x => x.Query, FieldTermVector.WithPositionsAndOffsets);
		}
	}
}