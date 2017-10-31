using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;

namespace WebSqlServerDocumentator.Controllers
{
	public class DatabaseController : Controller
	{
		private IDocumentator _documentator;

		public DatabaseController(IDocumentator documentator)
		{
			this._documentator = documentator;
		}

		[Route("{serverName}/databases")]
		public IActionResult Index(string serverName)
		{
			return View(this._documentator.GetDatabases(serverName));
		}

		[Route("/{serverName}/{databaseName}/tables")]
		public IActionResult Tables(string serverName, string databaseName)
		{
			return View(this._documentator.GetTables(serverName, databaseName));
		}

		[Route("/{serverName}/{databaseName}/views")]
		public IActionResult Views(string serverName, string databaseName)
		{
			return View(this._documentator.GetViews(serverName, databaseName));
		}
	}
}