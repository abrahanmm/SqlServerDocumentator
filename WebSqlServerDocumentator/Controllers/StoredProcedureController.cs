using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSqlServerDocumentator.Controllers
{
    public class StoredProcedureController: Controller
    {
        private IDocumentator _documentator;

        public StoredProcedureController(IDocumentator documentator)
        {
            this._documentator = documentator;
        }

        #region API
        [Route("/api/servers/{serverName}/databases/{databaseName}/storedProcedures")]
        public IActionResult GetAction(string serverName, string databaseName)
        {
            return Ok(this._documentator.GetStoredProcedures(serverName, databaseName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/storedProcedures/{storedProcedureName}")]
        public IActionResult GetAction(string serverName, string databaseName, string storedProcedureName)
        {
            return Ok(this._documentator.GetTables(serverName, databaseName));
        }
        #endregion
    }
}
