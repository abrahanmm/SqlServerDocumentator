using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSqlServerDocumentator.Models;
using SqlServerDocumentator;

namespace WebSqlServerDocumentator.Controllers
{
    public class HomeController : Controller
    {
        private IDocumentator Documentator;

        public HomeController()
        {
            this.Documentator = SqlServerDocumentator.Configuration.ConfigurationProvider.CreateInstance();
        }

        [Route("")]
        public IActionResult Index()
        {
            return View(this.Documentator.GetServers());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
