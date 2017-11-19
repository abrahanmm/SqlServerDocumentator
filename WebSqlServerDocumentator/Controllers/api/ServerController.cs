using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlServerDocumentator;
using SqlServerDocumentator.DocumentedDatabaseObjects;

namespace WebSqlServerDocumentator.Controllers.api
{
    public class ServerController : Controller
    {
        private IDocumentator _documentator;

        public ServerController(IDocumentator documentator)
        {
            this._documentator = documentator;
        }

        #region API
        [Route("/api/servers")]
        public IActionResult GetAction()
        {
            return Ok(this._documentator.GetServers());
        }
        #endregion
    }
}
