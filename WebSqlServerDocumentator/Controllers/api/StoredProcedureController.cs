using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSqlServerDocumentator.Controllers.api
{
    public class StoredProcedureController: Controller
    {
        private IDocumentator _documentator;

        public StoredProcedureController(IDocumentator documentator)
        {
            this._documentator = documentator;
        }
        
        [Route("/api/servers/{serverName}/databases/{databaseName}/storedProcedures")]
        public IActionResult GetAction(string serverName, string databaseName)
        {
            return Ok(this._documentator.GetStoredProcedures(serverName, databaseName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/storedProcedures/{storedProcedureName}")]
        public IActionResult GetAction(string serverName, string databaseName, string storedProcedureName)
        {
            return Ok(this._documentator.GetStoredProcedure(serverName, databaseName, "dbo", storedProcedureName));
        }

        [Route("/api/servers/{serverName}/databases/{databaseName}/storedProcedures/{schemaName}.{storedProcedureName}")]
        public IActionResult GetAction(string serverName, string databaseName, string schemaName, string storedProcedureName)
        {
            return Ok(this._documentator.GetStoredProcedure(serverName, databaseName, schemaName, storedProcedureName));
        }
    }
}
