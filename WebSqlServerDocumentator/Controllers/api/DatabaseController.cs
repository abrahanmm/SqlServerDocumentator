using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSqlServerDocumentator.Controllers.api
{
    public class DatabaseController: Controller
    {
        private IDocumentator _documentator;

        public DatabaseController(IDocumentator documentator)
        {
            this._documentator = documentator;
        }

        [Route("/api/servers/{serverName}/databases")]
        public IActionResult GetAction(string serverName)
        {
            return Ok(this._documentator.GetDatabases(serverName));
        }
    }
}
