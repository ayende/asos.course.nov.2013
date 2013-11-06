using System;

namespace Asos.Course.Models
{
	public class Car
	{
		 public string Id { get; set; }
		 public string Make { get; set; }
		 public string OwnerId { get; set; }
		 public string Model { get; set; }
		 public long Milage { get; set; }
		 public string Type { get; set; }
		 public DateTime OnRoadDate { get; set; }
		 public string Color { get; set; }
	}



	public class Person
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}