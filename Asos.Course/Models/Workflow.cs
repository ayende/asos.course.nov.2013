using System.CodeDom;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using Raven.Abstractions.Indexing;
using Raven.Client.Connection;
using Raven.Client.Indexes;
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

		public ColorItem[] Colors { get; set; }
	}

	public class ColorItem
	{
		public string Name { get; set; }
		public string Size { get; set; }
		public bool Active { get; set; }
	}

	public class WorkflowColors : AbstractIndexCreationTask<Workflow>
	{
		public class Result
		{
			public bool Active { get; set; }
			public string Size { get; set; }
			public string Brand { get; set; }
		}

		public WorkflowColors()
		{
			Map = workflows =>
				from workflow in workflows
				from color in workflow.Colors
				select new
				{
					color.Active,
					color.Size,
					workflow.Brand
				};

			StoreAllFields(FieldStorage.Yes);
		}
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