using System.Drawing;
using System.Security.Principal;
using Raven.Client.Connection;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace Asos.Course.Models
{
	public class Workflow
	{
		public string Status { get; set; }
		public string Name { get; set; }
		public string Colour { get; set; }
		public string DestinationDescription { get; set; }
		public string Path { get; set; }
		public string Priority { get; set; }
		public string Brand { get; set; }
		public string SampleReceiveDate { get; set; }
		public string DeliveryDate { get; set; }
		public double DaysInProduction { get; set; }
		public string FilenameStub { get; set; }
		public string Id { get; set; }
	}

	public class AuditListener : IDocumentStoreListener
	{
		public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
		{
			if (entityInstance is Workflow == false)
				return false;

			metadata["Modified-By"] = WindowsIdentity.GetCurrent().Name;
			return false;
		}

		public void AfterStore(string key, object entityInstance, RavenJObject metadata)
		{
		}
	}
}