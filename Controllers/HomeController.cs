using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Test.Models;
using Test.Dal;
using Microsoft.AspNetCore.Http;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;

        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }
        public ActionResult Login()
        {
            return View();
        }

        private readonly DataContext _dataContext;
        public HomeController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if(ModelState.IsValid)
            {
                foreach(User dbUser in _dataContext.Users)
                {
                    if(user.Password == dbUser.Password && user.Username == dbUser.Username)
                    {
                        HttpContext.Session.SetString("UserID", dbUser.UserId);
                        HttpContext.Session.SetString("Username", dbUser.Username);
                        return RedirectToAction("UserDashboard");
                    }
                }
            }
            TempData["Message"] = "Invalid username or password";
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("UserDashboard");
            }
            return View();
        }

        public IActionResult UserDashboard()
        {
            if(HttpContext.Session.GetString("UserID") != null)
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
                return View();
            }
            else
            {
                return RedirectToAction("Index");
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
