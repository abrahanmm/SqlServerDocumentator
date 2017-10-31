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
		private IDocumentator Documentator;

		public DatabaseController()
		{
			this.Documentator = SqlServerDocumentator.Configuration.ConfigurationProvider.CreateInstance();
		}

		[Route("{serverName}/databases")]
		public IActionResult Index(string serverName)
		{
			return View(this.Documentator.GetDatabases(serverName));
		}

		[Route("/{serverName}/{databaseName}/tables")]
		public IActionResult Tables(string serverName, string databaseName)
		{
			return View(this.Documentator.GetTables(serverName, databaseName));
		}

		[Route("/{serverName}/{databaseName}/views")]
		public IActionResult Views(string serverName, string databaseName)
		{
			return View(this.Documentator.GetViews(serverName, databaseName));
		}

		[Route("/{serverName}/{databaseName}/storedProcedures")]
		public IActionResult StoredProcedures(string serverName, string databaseName)
		{
			return View(this.Documentator.GetStoredProcedures(serverName, databaseName));
		}
	}
}