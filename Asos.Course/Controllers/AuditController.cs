using Asos.Course.Models;
using Raven.Client.Bundles.Versioning;

namespace Asos.Course.Controllers
{
	public class AuditController : AbstractController
	{
		public object Create()
		{
			Session.Store(new Workflow
			{
				Name = "text"
			});
			return Json("done");
		}

		public object Change(int id)
		{

			return Json("done");
		}

		public object Load(int id)
		{
			var style = Session.Load<Workflow>(id);

			return Json(new
			{
				Revisions = Session.Advanced.GetRevisionIdsFor<Workflow>(style.Id, 0, 25),
				Style = style
			});
		}
	}
}