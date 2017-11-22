using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using SqlServerDocumentator.DocumentedDatabaseObjects;
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

        [Route("/api/servers/{serverName}/databases/{databaseName}/views/{viewName}")]
        [HttpPut]
        public IActionResult PutAction(string serverName, string databaseName, string viewName, [FromBody] DocumentedView view)
        {
            if (!serverName.Equals(view.ServerName) ||
                !databaseName.Equals(view.DatabaseName) ||
                !"dbo".Equals(view.Schema) ||
                !viewName.Equals(view.Name))
                return BadRequest("Exist a mismatch between the url and json data.");
            return Ok(this._documentator.SaveView(view));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/views/{schema}.{viewName}")]
        [HttpPut]
        public IActionResult PutAction(string serverName, string databaseName, string schema, string viewName, [FromBody] DocumentedView view)
        {
            if (!serverName.Equals(view.ServerName) ||
                !databaseName.Equals(view.DatabaseName) ||
                !schema.Equals(view.Schema) ||
                !viewName.Equals(view.Name))
                return BadRequest("Exist a mismatch between the url and json data.");
            return Ok(this._documentator.SaveView(view));
        }
    }
}
