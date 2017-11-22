using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using SqlServerDocumentator.DocumentedDatabaseObjects;

namespace WebSqlServerDocumentator.Controllers.api
{
    public class TableController : Controller
    {
        private IDocumentator _documentator;

        public TableController(IDocumentator documentator)
        {
            this._documentator = documentator;
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/tables")]
        public IActionResult GetAction(string serverName, string databaseName)
        {
            return Ok(this._documentator.GetTables(serverName, databaseName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/tables/{tableName}")]
        public IActionResult GetAction(string serverName, string databaseName, string tableName)
        {
            return Ok(this._documentator.GetTable(serverName, databaseName, "dbo", tableName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/tables/{schema}.{tableName}")]
        public IActionResult GetAction(string serverName, string databaseName, string schema, string tableName)
        {
            return Ok(this._documentator.GetTable(serverName, databaseName, schema, tableName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/tables/{tableName}")]
        [HttpPut]
        public IActionResult PutAction(string serverName, string databaseName, string tableName, [FromBody] DocumentedTable table)
        {
            if (!serverName.Equals(table.ServerName) ||
                !databaseName.Equals(table.DatabaseName) ||
                !"dbo".Equals(table.Schema) ||
                !tableName.Equals(table.Name))
                return BadRequest("Exist a mismatch between the url and json data.");
            return Ok(this._documentator.SaveTable(table));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/tables/{schema}.{tableName}")]
        [HttpPut]
        public IActionResult PutAction(string serverName, string databaseName, string schema, string tableName, [FromBody] DocumentedTable table)
        {
            if (!serverName.Equals(table.ServerName) ||
                !databaseName.Equals(table.DatabaseName) ||
                !schema.Equals(table.Schema) ||
                !tableName.Equals(table.Name))
                return BadRequest("Exist a mismatch between the url and json data.");
            return Ok(this._documentator.SaveTable(table));
        }
    }
}
