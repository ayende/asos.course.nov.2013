using System.Linq;
using Raven.Client.Indexes;

namespace Asos.Course.Models
{
	public class Person
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}

	public class DogsAndOwnersTransformer : AbstractTransformerCreationTask<Dog>
	{
		public class Result
		{
			public string Name;
			public string[] Owners;
		}

		public DogsAndOwnersTransformer()
		{
			TransformResults = dogs =>
				from dog in dogs
				select new
				{
					dog.Name,
					Owners = dog.Owners.Select(x => LoadDocument<Person>(x).Name)
				};
		}
	}
}