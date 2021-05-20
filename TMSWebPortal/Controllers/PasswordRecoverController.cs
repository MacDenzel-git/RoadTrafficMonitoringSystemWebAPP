using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TMSWebPortal.DTO;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TMSWebPortal.Controllers;

namespace LeadersPortalWebAPP.Controllers
{
    public class PasswordRecoverController : Controller
    {
        const string SessionPosition = "position";
        const string SessionUserName = "name";
        const string RoleId = "";
        private readonly IConfiguration _configuration;
        static string apiUri = "PasswordRecover";
        // const string SessionUserName = "user";
      //  private readonly IHttpContextAccessor _httpContextAccessor;
        public PasswordRecoverController(IConfiguration configuration)
        {
            _configuration = configuration;
           // _httpContextAccessor = httpContextAccessor;
        }

        public string BaseUrl
        {
            get
            {
                return _configuration["EndpointURL"];
            }
        }

        [HttpGet]
        public async  Task<IActionResult> ResetPassword()
        {
            LoginViewModel loginViewModel = new LoginViewModel
            {
                IsErrorOccured = false
            };
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(LoginViewModel loginViewModel)
        {
            ResultHandler resultHandler = new ResultHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/ResetPassword";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PostAsJsonAsync(requestUrl, loginViewModel);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    resultHandler = await response.Content.ReadAsAsync<ResultHandler>();

                    return View(new LoginViewModel { IsErrorOccured = resultHandler.IsErrorOccured, Message = resultHandler.Message });

                }
                else
                {
                    resultHandler = await response.Content.ReadAsAsync<ResultHandler>();

                    LoginViewModel result = new LoginViewModel
                    {
                        IsErrorOccured = true,
                        Message = resultHandler.Message
                     };
                    return View(result);
                };
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecoverPassword()
        {

            RecoverPasswordDTO recoverPasswordDTO = new RecoverPasswordDTO
            {
                IsErrorOccured = false
            };
            return View(recoverPasswordDTO);
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDTO recoverPasswordDTO)
        {
            ResultHandler output = new ResultHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/RecoverPassword";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PostAsJsonAsync(requestUrl, recoverPasswordDTO);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsAsync<ResultHandler>();
                    if(result.IsErrorOccured)
                    {
                        return View(new RecoverPasswordDTO { IsErrorOccured = result.IsErrorOccured,Message = result.Message });
                    }
                    int? value = 0;

                  
                    var details = JsonConvert.DeserializeObject<PersonalDetails>(result.Result.ToString());

                    output =  IsLoggedIn(details.PositionId, details.EmailAddress, details.RoleId);
                    if (output.Message.Equals("SESSION_SET"))
                    {
                        value = output.Result as int?;
                        //if the session has been set do nothing, just proceed to get sermons
                    }
                    else
                    {
                        if (output.Result == null)
                        {
                            return RedirectToAction("Login", "Accounts");
                        }
                        else
                        {
                            value = output.Result as int?;
                        }
                    }

                    bool isRoleSet = false;
                    if (isRoleSet)
                    {

                    }

                    return RedirectToAction("ChangePassword","PasswordRecover",details.EmailAddress);

                }
                else
                {
                    return View(new RecoverPasswordDTO { IsErrorOccured = true, Message = "Something went Wrong" });
                };
            }
        }

         [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            string username = HttpContext.Session.GetString("name");
            if (username == null)
            {
                RedirectToAction("Accounts", "Login");
            }
         
            LoginViewModel recoverPasswordDTO = new LoginViewModel
            {
                IsErrorOccured = false,
            };

           
            return View(recoverPasswordDTO);
        }

        [HttpPost]

        public async Task<IActionResult> ChangePassword(LoginViewModel loginViewModel)
        {
            loginViewModel.Username = HttpContext.Session.GetString("name");
            ResultHandler resultHandler = new ResultHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/ChangePassword";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PostAsJsonAsync(requestUrl, loginViewModel);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    resultHandler = await response.Content.ReadAsAsync<ResultHandler>();

                    return RedirectToAction("Login", "Accounts");


                }
                else
                {
                    resultHandler = await response.Content.ReadAsAsync<ResultHandler>();
                    return View(new LoginViewModel { IsErrorOccured = resultHandler.IsErrorOccured, Message = resultHandler.Message });
                };
            }
        }

        public ResultHandler IsLoggedIn(int positionId, string username, int role, int? sessionValue = 0)
        {
            ResultHandler resultHandler = new ResultHandler();

            if (positionId != 0)
            {
                //if position is not 0 user is logging on and set up the session
                HttpContext.Session.SetInt32(SessionPosition, positionId);
                sessionValue = HttpContext.Session.GetInt32(SessionPosition);

                HttpContext.Session.SetString(SessionUserName, username);
                ViewBag.username = HttpContext.Session.GetString(SessionUserName);

                HttpContext.Session.SetInt32(RoleId, role);
                ViewBag.role = HttpContext.Session.GetInt32(RoleId);

                HttpContext.Session.SetInt32(SessionPosition, positionId);
                resultHandler.Message = "SESSION_SET";
                resultHandler.Result = positionId;
            }
            else
            {
                //if position is 0 this is not the first request from login user had logged in
                //you now just need to get the position from the current user session 
                sessionValue = HttpContext.Session.GetInt32(SessionPosition);
                string sessionUser = HttpContext.Session.GetString(SessionUserName);
                ViewBag.role = HttpContext.Session.GetInt32(RoleId);

                if (sessionValue == null)
                {
                    resultHandler.IsErrorOccured = true;
                    resultHandler.Message = "User has not logged in";

                }
                else
                {
                    resultHandler.IsErrorOccured = false;
                    resultHandler.Result = sessionValue;
                    resultHandler.Message = "SESSION_VALUE_RETRIEVED";
                }

            }
            return resultHandler;
        }
    }
}