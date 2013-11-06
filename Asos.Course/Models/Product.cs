using System.Collections.Generic;

namespace Asos.Course.Models
{
	public class Product
	{
		public string Id { get; set; } 
		public string Name { get; set; }
		public Dictionary<string,string> Properties { get; set; }

		// create an index to search products by name
		// create an index to search for products by property
		// Coat product - Size: M
		// Shoe product - Heel: 10in
	}
}