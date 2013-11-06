using System.Linq;
using Asos.Course.Models;
using Raven.Client.Indexes;

namespace Asos.Course.Indexes
{
	public class Search_ByName : AbstractMultiMapIndexCreationTask
	{
		public Search_ByName()
		{
			AddMap<Customer>(customers => 
				from c in customers select new { c.Name });
			AddMap<Lead>(leads => 
				from l in leads select new { l.Name });
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