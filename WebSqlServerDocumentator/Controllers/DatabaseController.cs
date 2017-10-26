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

        [Route("/server/{serverName}")]
        public IActionResult Index(string serverName)
        {
            return View(this.Documentator.GetDatabases(serverName));
        }
    }
}