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
        private IDocumentator _documentator;

        public HomeController(IDocumentator documentator)
        {
            _documentator = documentator;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View(_documentator.GetServers());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
