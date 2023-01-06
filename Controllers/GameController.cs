using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDD_ASG2.DAL;
using SDD_ASG2.Models;
using SDD_ASG2.ViewModels;
using System.Text.RegularExpressions;

namespace SDD_ASG2.Controllers
{
    public class GameController : Controller
    {

        private UserDAL userContext = new UserDAL();
        private ScoresDAL scoreContext = new ScoresDAL();

        // GET: Game
        public ActionResult Index(string id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role == "User")
            {

                ViewData["SavedGameData"] = "{}";
                //play new game
                if (id == "new")
                {
                    return View();
                }
                //resume saved game
                else if (id == "resume")
                {
                    int userid = (int)HttpContext.Session.GetInt32("UserId");
                    ViewData["SavedGameData"] = userContext.GetGameData(userid);
                    return View();
                }
                //wrong url
                else
                {
                    TempData["UserNotLoggedIn"] = "Login is required";
                    return RedirectToAction("Index", "Home");
                }
            }
            //user not logged in
            else
            {
                TempData["UserNotLoggedIn"] = "Login is required";
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult SaveData(IFormCollection collection)
        {

            string gamedata = collection["data"];
            if (gamedata == null)
            {
                return StatusCode(404);
            }

            int userid = (int)HttpContext.Session.GetInt32("UserId");
            userContext.SaveGameData(userid, gamedata);
            return Ok(200);
        }

        [HttpPost]
        public ActionResult Finish(IFormCollection collection)
        {
            string score = collection["data"];
            if (score == null)
            {
                return StatusCode(404);
            }

            int userid = (int)HttpContext.Session.GetInt32("UserId");
            scoreContext.InsertScore(userid, int.Parse(score));

            int topPosition = scoreContext.CheckInLeaderboard(userid);
            //check score in top 10
            if (topPosition != 0)
            {
                TempData["FinishScore"] = $"You are the top {topPosition} position in the leaderboard </br> You scored {score} points";
                return StatusCode(201);
            }
            else
            {
                TempData["FinishScore"] = $"You scored {score} points";
                return Ok();
            }
        }


        public ActionResult Leaderboard()
        {
            List<Scores> scores = scoreContext.GetHighscores();
            return View(scores);
        }

        public ActionResult Profile()
        {
            string role = HttpContext.Session.GetString("Role");
            if (role == "User")
            {
                int userid = (int)HttpContext.Session.GetInt32("UserId");
                User currentUser = userContext.GetUser(userid);
                int highscore = scoreContext.GetUserHighscore(userid);
                TempData["username"] = currentUser.Username;
                TempData["highscore"] = highscore;
                return View();
            }
            else
            {
                TempData["UserNotLoggedIn"] = "Login is required";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(IFormCollection formdata)
        {
            string currentPW = formdata["currentPW"].ToString().Trim();
            string newPW = formdata["newPW"].ToString().Trim();
            string cNewPW = formdata["cNewPW"].ToString().Trim();

            // Get current user
            int userid = (int)HttpContext.Session.GetInt32("UserId");
            User currentUser = userContext.GetUser(userid);

            // Validation for change of password
            if (userContext.CheckPassword(currentUser.Email, currentPW))
            {
	            if (newPW != cNewPW)
                {
                    TempData["cNewPWError"] = "Confirmation password is not the same as new password!";
                }
                else if (!Regex.Match(newPW, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z/\\d]{8,}$", RegexOptions.IgnoreCase).Success && newPW.Length < 128)
                {
                    TempData["newPWError"] = "Password must have minimum eight characters, at least one uppercase letter, one lowercase letter and one number";
                }
                else
                {
                    userContext.ChangePassword(userid, newPW);
                }
                            
            }
            else
            {
                TempData["oldPWError"] = "Current Password does not match!";
            }

            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsername(IFormCollection formdata)
        {
            string newUsername = formdata["newUsername"].ToString().Trim();

            // Get current user
            int userid = (int)HttpContext.Session.GetInt32("UserId");

            // check if current username exist
            if (!userContext.IsUsernameExist(newUsername) && newUsername != "")
            {
                userContext.ChangeUsername(userid, newUsername);
            }
            else
            {
                TempData["usernameError"] = "Username already exist!";
            }

            return RedirectToAction("Profile");
        }

    }
}
