﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDD_ASG2.DAL;
using SDD_ASG2.Models;

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


        public ActionResult Leaderboard()
        {
            List<Scores> scores = scoreContext.GetHighscores();
            return View(scores);
        }

        public ActionResult Profile()
        {
            return View();
        }

    }
}
