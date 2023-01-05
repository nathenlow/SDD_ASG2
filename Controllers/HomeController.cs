using Microsoft.AspNetCore.Authentication.Cookies;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Google.Apis.Auth;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

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

        //google sign in
        [Authorize]
        public async Task<ActionResult> GoogleRegister()
        {
            // The user is already authenticated, so this call won't
            // trigger login, but it allows us to access token related values.
            AuthenticateResult auth = await HttpContext.AuthenticateAsync();
            string idToken = auth.Properties.GetTokenValue(
            OpenIdConnectParameterNames.IdToken);
            try
            {
                // Verify the current user logging in with Google server
                // if the ID is invalid, an exception is thrown
                Payload currentUser = await
                GoogleJsonWebSignature.ValidateAsync(idToken);
                string userName = currentUser.Name;
                string eMail = currentUser.Email;
                int userid;
                // check if user email exist in database
                if (userContext.IsEmailExist(eMail))
                {
                    userid = userContext.GetUserId(eMail);
                    HttpContext.Session.SetInt32("UserId", userid);
                    HttpContext.Session.SetString("Role", "User");
                    return RedirectToAction("Index", "Home");
                }

                if (userContext.IsUsernameExist(userName))
                {
                    userContext.SSORegister(eMail);
                }
                else
                {
                    userContext.SSORegister(eMail, userName);
                }

                userid = userContext.GetUserId(eMail);
                HttpContext.Session.SetInt32("UserId", userid);
                HttpContext.Session.SetString("Role", "User");
                return RedirectToAction("Index", "Home");
                
            }
            catch (Exception e)
            {
                // Token ID is may be tempered with, force user to logout
                return RedirectToAction("Index", "Logout");
            }


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

            string email = formData["email"].ToString().Trim().ToLower();
            string password = formData["password"].ToString().Trim();
            if (password.Length >= 8 && userContext.CheckPassword(email, password))
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
