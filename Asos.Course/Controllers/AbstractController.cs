using System;
using System.Reflection;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Asos.Course.Controllers
{
	public class AbstractController : Controller
	{
		 private static readonly Lazy<IDocumentStore> _documentStoreLazy = new Lazy<IDocumentStore>(() =>
		 {
			 var store = new DocumentStore
			 {
				 ConnectionStringName = "RavenDB"
			 };
			 store.Initialize();

			 IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);

			 return store;
		 });

		 public IDocumentStore DocumentStore { get { return _documentStoreLazy.Value; } }

		 public new IDocumentSession Session { get; set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			Session = DocumentStore.OpenSession();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			using (Session)
			{
				if (Session == null || filterContext.Exception != null)
					return;
				Session.SaveChanges();
			}
		}
	}
}