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
using SDD_ASG2.ViewModels;

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
            return View();
        }


        public ActionResult Register()
        {
            string role = HttpContext.Session.GetString("Role");
            switch (role)
            {
                case "User":
                    return RedirectToAction("Index", "Home");
                default:
                    return View();
            }
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegister newUser)
        {
            if (ModelState.IsValid)
            {
                userContext.Register(newUser);
                int userid = userContext.GetUserId(newUser.Email);
                HttpContext.Session.SetInt32("UserId", userid);
                HttpContext.Session.SetString("Role", "User");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Input validation fails, return to the Register view to display error message
                return View(newUser);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public ActionResult Login()
        {
            string role = HttpContext.Session.GetString("Role");
            switch (role)
            {
                case "User":
                    return RedirectToAction("Index", "Home");
                default:
                    return View();
            }
        }

        [HttpPost]
        public IActionResult Login(IFormCollection formData)
        {
            // LoginID converted to lowercase

            string email = formData["email"].ToString();
            string password = formData["password"].ToString();
            if (userContext.CheckPassword(email, password))
            {
                int userid = userContext.GetUserId(email);
                HttpContext.Session.SetInt32("UserId", userid);
                HttpContext.Session.SetString("Role", "User");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Store an error message in TempData for display at the login page
                TempData["ErrorMsg"] = "Invalid Login Credentials!";
                return View();
            }
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

    }
}
