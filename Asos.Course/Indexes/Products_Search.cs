using System.Linq;
using Asos.Course.Models;
using Raven.Client.Indexes;

namespace Asos.Course.Indexes
{
	public class Search_ByName : AbstractMultiMapIndexCreationTask<Search_ByName.Result>
	{
		public class Result
		{
			public int Count;
			public string Name;
		}
		public Search_ByName()
		{
			AddMap<Customer>(customers => 
				from c in customers select new { c.Name, Count =1 });
			AddMap<Lead>(leads => 
				from l in leads select new { l.Name, Count =1 });

			Reduce = results =>
				from result in results
				group result by result.Name
				into g
				select new {Count = g.Sum(x => x.Count), Name = g.Key};
		}
	}

	public class Products_Search : AbstractIndexCreationTask<Product, Products_Search.Result>
	{
		public class Result
		{
		}


		public Products_Search()
		{
			Map = products =>
				from product in products
				select new
				{
					product.Name,
					_ = product.Properties.Select(x => CreateField("Properties_" + x.Key, x.Value))
				};
		}
	}
}