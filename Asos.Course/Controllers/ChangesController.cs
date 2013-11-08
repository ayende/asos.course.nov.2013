using System;
using System.Linq;
using System.Media;
using Asos.Course.Models;

namespace Asos.Course.Controllers
{
	public class ChangesController : AbstractController
	{
		public object SetupChanges()
		{
			DocumentStore.Changes()
				.ForAllDocuments()
				.Subscribe(notification => new SoundPlayer(@"http://www.shockwave-sound.com/sound-effects/scream-sounds/2scream.wav").Play());

			return Json("ok");
		}

		public object Caching()
		{
			using (DocumentStore.AggressivelyCache())
			{
				return Json(Session.Load<Workflow>(1));
			}
		}
	}

}