using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coba.AntiXSSMiddleware.Models;
using Coba.AntiXSSMiddleware.ActionFilter;

namespace Coba.AntiXSSMiddleware.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        //[AntiXSSFilter]
        public IActionResult Index(MyTestViewModel model)
        {
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View("ErrorSuspiciousXSS");
        }

        public IActionResult ErrorXssAjax()
        {
            return Json(new
            {
                IsSuccess = "false",
                Message = "Xss Suspicious Attack"
            });
        }
    }
}
