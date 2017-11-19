using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using SqlServerDocumentator.DocumentedDatabaseObjects;

namespace WebSqlServerDocumentator.Controllers.api
{
    public class TableController: Controller
    {
        private IDocumentator _documentator;

        public TableController(IDocumentator documentator)
        {
            this._documentator = documentator;
        }

        #region API
        [Route("/api/servers/{serverName}/databases/{databaseName}/tables")]
        public IActionResult GetAction(string serverName, string databaseName)
        {
            return Ok(this._documentator.GetTables(serverName, databaseName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/tables/{tableName}")]
        public IActionResult GetAction(string serverName, string databaseName, string tableName)
        {
            return Ok(this._documentator.GetTables(serverName, databaseName));
        }
        #endregion
    }
}
