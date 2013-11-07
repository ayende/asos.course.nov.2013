using System.Linq;
using Asos.Course.Models;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Bundles.MoreLikeThis;

namespace Asos.Course.Controllers
{
	public class WorkflowController : AbstractController
	{
		public object Search(string q, string b)
		{
			var query = Session.Query<Workflows_Search.Result, Workflows_Search>()
				.Where(x => x.DaysInProduction > 0.5);

			if (string.IsNullOrEmpty(q) == false)
				query = query.Search(x => x.Query, q);

			if (string.IsNullOrEmpty(b) == false)
				query = query.Search(x => x.Query, b, boost: 0.9m);

			var results = query
				.As<Workflow>()
				.ToList();

			if (results.Count == 0 && string.IsNullOrEmpty(q) == false)
			{
				var suggest = Session.Query<Workflows_Search.Result, Workflows_Search>()
					.Search(x=>x.Query, q)
					.Suggest();
				switch (suggest.Suggestions.Length)
				{
					case 0:
						return Json("Nothing here!");
					case 1:
						return Search(suggest.Suggestions[0],b);
					default:
						return Json(new { DidYouMean = suggest.Suggestions });
				}
			}

			return Json(new
			{
				Query = query.ToString(),
				Results = results.Select(r => new
				{
					r.Name,r.Brand,
					Score = Session.Advanced.GetMetadataFor(r).Value<float>("Temp-Index-Score")
				})
			});
		}

		public object MoreLikeThis(string id)
		{
			var results = Session.Advanced.MoreLikeThis<Workflow>(new Workflows_Search().IndexName, new MoreLikeThisQuery
			{
				DocumentId = id,
				MinimumDocumentFrequency = 2,
				MinimumTermFrequency = 1,
				MinimumWordLength = 3,
				Fields = new []{"Query"},
			});

			return Json(new
			{
				Results = results.Select(r => new
				{
					r.Name,
					r.Brand,
				})
			});
		}
	}
}