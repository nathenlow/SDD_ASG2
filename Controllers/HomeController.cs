using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDD_ASG2.DAL;
using SDD_ASG2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;

namespace SDD_ASG2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserDAL userContext = new UserDAL();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            User user = userContext.getUser("n@l.com");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult Register()
        {
            User user = new User();
            return View(user);
        }

        // POST: Register
        [HttpPost]
        public ActionResult Registered(User user)
        {
            //Add staff record to database
            userContext.Register(user);
            //Redirect user to Customer/Index view
            return RedirectToAction("Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public ActionResult Login()
        {
            return View();
        }

       

    }
}
