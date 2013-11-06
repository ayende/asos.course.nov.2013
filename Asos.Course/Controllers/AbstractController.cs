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
				 Url = "http://localhost:8787",
				 DefaultDatabase = "Asos.Course"
			 };
			 store.Initialize();

			 IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);

			 return store;
		 });

		 public IDocumentStore DocumentStore { get { return _documentStoreLazy.Value; } }
	}
}