using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSqlServerDocumentator.Controllers.api
{
	public class ViewController : Controller
	{
		private IDocumentator _documentator;

		public ViewController(IDocumentator documentator)
		{
			this._documentator = documentator;
		}

		[Route("/api/servers/{serverName}/databases/{databaseName}/views")]
		public IActionResult GetAction(string serverName, string databaseName)
		{
			return Ok(this._documentator.GetViews(serverName, databaseName));
		}

        [Route("/api/servers/{serverName}/databases/{databaseName}/views/{viewName}")]
        public IActionResult GetAction(string serverName, string databaseName, string viewName)
        {
            return Ok(this._documentator.GetView(serverName, databaseName, "dbo", viewName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/views/{schemaName}.{viewName}")]
		public IActionResult GetAction(string serverName, string databaseName, string schemaName, string viewName)
		{
			return Ok(this._documentator.GetView(serverName, databaseName, schemaName, viewName));
		}
	}
}
