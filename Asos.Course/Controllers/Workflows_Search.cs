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
		}

		public Workflows_Search()
		{
			Map = workflows => from wf in workflows
				select new
				{
					wf.DaysInProduction,
					Query = new[] {wf.Name, wf.Brand}
				};

			Index(x => x.Query, FieldIndexing.Analyzed);
			Suggestion(x=>x.Query, new SuggestionOptions
			{
				Accuracy = 0.5f,
				Distance = StringDistanceTypes.Default
			});
			TermVector(x=>x.Query, FieldTermVector.WithPositionsAndOffsets);
		}
	}
}