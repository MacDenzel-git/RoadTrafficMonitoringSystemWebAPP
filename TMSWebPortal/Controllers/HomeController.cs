using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMSWebPortal.DTO;
using TMSWebPortal.Models;
using LoginViewModel = TMSWebPortal.DTO.LoginViewModel;

namespace TMSWebPortal.Controllers
{
    public class HomeController : Controller
    {
        public const string SessionKeyName = "user";
        public const string SessionKeyRole = "position";
        
        public async Task<IActionResult> Index(string user, int roleid)
        {
            var cookieValue = "";
            var  role = 1;
            if (user == null)
            {
                 cookieValue = HttpContext.Session.GetString("user");
                  role = Convert.ToInt32(HttpContext.Session.GetInt32("position"));
            }
            else
            {
                HttpContext.Session.SetString(SessionKeyName, user);
                     HttpContext.Session.SetInt32(SessionKeyRole, roleid);
            }
        


             cookieValue = HttpContext.Session.GetString("user");
            role = Convert.ToInt32( HttpContext.Session.GetInt32("position"));

            var homemodel = new GeneralFilter
            {
                roleid = role,
                username = cookieValue,
                Crimes = await StaticDataHandler.GetCrimes("https://localhost:44311/api/")
            };
            return View(homemodel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }


        public IActionResult Login(LoginViewModel loginRequest)
        {
            if (loginRequest.IsErrorOccured)
            {
                return View(loginRequest);
            }

            loginRequest.Message = "";
            return View(loginRequest);
        }
        public IActionResult Authenticate(LoginViewModel loginRequest)
        {

            if (loginRequest.Username.Equals("Denzel") && loginRequest.Password.Equals("Machowa"))
            {
                HttpContext.Session.SetString(SessionKeyName, "Denzel");
                var user = HttpContext.Session.GetString("user");
                return RedirectToAction("Index","Home",user);
 
            }

            LoginViewModel loginViewModel = new LoginViewModel
            {
                IsErrorOccured = true,
                Message = $"username or password is not correct, please try again",
            };
         

            return RedirectToAction(nameof(Login),loginViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var cookieValue = "";
            var role = 1;

            cookieValue = HttpContext.Session.GetString("user");
            role = Convert.ToInt32(HttpContext.Session.GetInt32("position"));
            if (role == 0 || cookieValue == null)
            {
                RedirectToAction("Login");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
