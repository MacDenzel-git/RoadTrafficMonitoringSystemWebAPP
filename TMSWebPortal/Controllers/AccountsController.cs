
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TMSWebPortal.DTO;
using TMSWebPortal.Models;

namespace TMSWebPortal.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;
        static string apiUri = "Accounts";
        // const string SessionUserName = "user";
        const string SessionUserName = "name";
        public const string SessionKeyRole = "position";
        public string BaseUrl
        {
            get
            {
                return _configuration["EndpointURL"];
            }
        }

        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PersonalDetails> users = new List<PersonalDetails>();
            var requestUrl = $"{BaseUrl}{apiUri}/GetAllUsers";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    users = await response.Content.ReadAsAsync<IEnumerable<PersonalDetails>>();

                };

            };

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register(string message )
        {

            if (message != null)
            {
                ViewBag.message = message;
            }

            IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
            IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
            IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);

            var user = new UserDTO
            {
                Branches = branches,
                Positions = positions,
                Roles = roles,
            
            };

            return View(user);
        }

        public async Task<IActionResult> Register(UserDTO user)
        {
            if (user.DateOfBirth > DateTime.UtcNow.AddHours(2))
            {
                string error = "Date cannot be from the future";
                return RedirectToAction("Register", new { message = error });

            }


            if (user.DateOfBirth == DateTime.UtcNow.AddHours(2))
            {
                string error = "Date cannot be today";
                return RedirectToAction("Register", new { message = error });

            }

            int age = DateTime.UtcNow.Year - user.DateOfBirth.Year;
            if (age < 16)
            {
                string error = "Minimum years is 16 years old";
                return RedirectToAction("Register", new { message = error });
            }
            
            OutputHandler resultHandler = new OutputHandler();
            if (user.Password == user.ConfirmPassword)
            {
                if (ModelState.IsValid)
                {
                    var requestUrl = $"{BaseUrl}{apiUri}/CreateUser";
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(requestUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        user.RoleId = 2;


                        var result = await client.PostAsJsonAsync(client.BaseAddress, user);

                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            if (user.PositionId == 3)
                            {
                                return RedirectToAction("ManageUsers");
                            }
                            else
                            {
                                return RedirectToAction("ManageOfficers");
                            }

                        }
                        else
                        {

                            resultHandler = await result.Content.ReadAsAsync<OutputHandler>();
                            if (resultHandler.Message != null)
                            {
                                user.Message = resultHandler.Message;
                            }

                            IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                            IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                            IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                            user.Branches = branches;
                            user.Positions = positions;
                            user.Roles = roles;
                            user.IsOfficerCreation = user.IsOfficerCreation;

                            return View(user);
                        }

                    };
                }
                else
                {
                    IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                    IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                    IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                    user.Branches = branches;
                    user.Positions = positions;
                    user.Roles = roles;
                    user.IsOfficerCreation = user.IsOfficerCreation;

                    return View(user);
                }
            }
            else
            {
                ModelState.AddModelError("", "Password does do not match");
                ;
                IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                user.Branches = branches;
                user.Positions = positions;
                user.Roles = roles;
                user.IsOfficerCreation = user.IsOfficerCreation;

                //var user = new UserDTO
                //{
                //    Branches = branches,
                //    Positions = positions,
                //    Roles = roles
                //};
            }
            return View(user);


        }

        [HttpGet]
        public async Task<IActionResult> RegisterOfficer(string message, bool isOfficerCreation = false)
        {

            if (message != null)
            {
                ViewBag.message = message;
            }

            IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
            IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
            IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);

            var user = new UserDTO
            {
                Branches = branches,
                Positions = positions,
                Roles = roles,
                Message = message
            };

            return View(user);
        }

        public async Task<IActionResult> RegisterOfficer(UserDTO user)
        {
            if (user.DateOfBirth > DateTime.UtcNow.AddHours(2))
            {
                string error = "Date cannot be from the future";
                return RedirectToAction("RegisterOfficer", new { message = error });

            }


            if (user.DateOfBirth == DateTime.UtcNow.AddHours(2))
            {
                string error = "Date cannot be today";
                return RedirectToAction("RegisterOfficer", new { message = error });

            }

            int age = DateTime.UtcNow.Year - user.DateOfBirth.Year;
            if (age < 16)
            {
                string error = "Minimum years is 16 years old";
                return RedirectToAction("RegisterOfficer", new { message = error });
            }
           
                if (user.Password == null || user.ConfirmPassword == null)
                {
                    return RedirectToAction("RegisterOfficer", new { message = "Password/Confirm Password cannot be empty" });
                }
       
            OutputHandler resultHandler = new OutputHandler();
            if (user.Password == user.ConfirmPassword)
            {
                if (ModelState.IsValid)
                {
                    var requestUrl = $"{BaseUrl}{apiUri}/CreateUser";
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(requestUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        user.RoleId = 1;


                        var result = await client.PostAsJsonAsync(client.BaseAddress, user);

                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                          
                                return RedirectToAction("ManageOfficers");
                          

                        }
                        else
                        {

                            resultHandler = await result.Content.ReadAsAsync<OutputHandler>();
                            if (resultHandler.Message != null)
                            {
                                user.Message = resultHandler.Message;
                            }

                            IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                            IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                            IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                            user.Branches = branches;
                            user.Positions = positions;
                            user.Roles = roles;
                            user.IsOfficerCreation = user.IsOfficerCreation;

                            return View(user);
                        }

                    };
                }
                else
                {
                    IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                    IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                    IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                    user.Branches = branches;
                    user.Positions = positions;
                    user.Roles = roles;
                    user.IsOfficerCreation = user.IsOfficerCreation;

                    return View(user);
                }
            }
            else
            {
                ModelState.AddModelError("", "Password does do not match");
                ;
                IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                user.Branches = branches;
                user.Positions = positions;
                user.Roles = roles;
                user.IsOfficerCreation = user.IsOfficerCreation;

                //var user = new UserDTO
                //{
                //    Branches = branches,
                //    Positions = positions,
                //    Roles = roles
                //};
            }
            return View(user);


        }
        [HttpGet]
        public IActionResult Login()
        {
            var viewModel = new UserCredentialsDTO
            {
                IsErrorOccured = false
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsDTO userCredentials)
        {


            if (ModelState.IsValid)
            {
                OutputHandler resultHandler = new OutputHandler();
                PersonalDetails personalDetails = new PersonalDetails();
                var requestUrl = $"{BaseUrl}{apiUri}/Login";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var result = await client.PostAsJsonAsync(client.BaseAddress, userCredentials);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        personalDetails = await result.Content.ReadAsAsync<PersonalDetails>();
                        if (personalDetails != null)
                        {
                            ViewBag.role = personalDetails.RoleId;
                            HttpContext.Session.SetString(SessionUserName, personalDetails.EmailAddress);
                            HttpContext.Session.SetInt32(SessionKeyRole, personalDetails.RoleId);
                            //get position abbr and name later 
                            return RedirectToAction("Index", "Home", new { roleid = personalDetails.RoleId, user = personalDetails.EmailAddress });

                        }
                        else
                        {
                            var userDTO = new UserCredentialsDTO
                            {
                                Message = "Account not found, Check your password or username and try again.",
                                IsErrorOccured = true

                            };

                            return View(userDTO);
                        }
                    }
                    else if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var userDTO = new UserCredentialsDTO
                        {
                            Message = "Login failed,Check your password and username and try again",
                            IsErrorOccured = true

                        };

                        return View(userDTO);
                    }
                    else
                    {


                        var userDTO = new UserCredentialsDTO
                        {
                            Message = "Something went wrong, Check you credentials and try again",
                            IsErrorOccured = true

                        };

                        return View(userDTO);
                    }
                };
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers(string username, int role)
        {

            username = HttpContext.Session.GetString("user");
            if (username == null)
            {
                HttpContext.Session.SetString(SessionUserName, username);
            }



            if (username == null && role != 1)
            {
                return RedirectToAction("Login", "Accounts");
            }
            UsersVM users = new UsersVM();
            var requestUrl = $"{BaseUrl}{apiUri}/GetAllUsers";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    users.UserDetailDTOs = await response.Content.ReadAsAsync<IEnumerable<UserDetailDTO>>();
                    users.UserDetailDTOs = users.UserDetailDTOs.Where(x => x.RoleId == 2);
                    var totals = await Totals();
                    users.CrimesCount = totals.CrimesCount;
                    users.Licenses = totals.Licenses;
                    users.CrimesToday = totals.CrimesToday;
                    users.VehiclesRegistered = totals.VehiclesRegistered;


                };

            };

            return View(users);
        }
        [HttpGet]
        public async Task<Totals> Totals()
        {

            Totals totals = new Totals();
            var requestUrl = $"{BaseUrl}{apiUri}/Count";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    totals = await response.Content.ReadAsAsync<Totals>();

                };

            };

            return totals;
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserCredentialStatus(long personalDetailsId, int roleId = 0)
        {
            var requestUrl = $"{BaseUrl}{apiUri}/ChangeUserCredentialStatus?personaldetailsId={personalDetailsId}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PutAsJsonAsync(requestUrl, personalDetailsId);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("ManageUsers", "", roleId);
                }
                else
                {
                    return RedirectToAction("ManageUsers");
                };

            }

        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserRole(int personalDetailsId)
        {
            var requestUrl = $"{BaseUrl}{apiUri}/ChangeUserRole?personaldetailsId={personalDetailsId}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PutAsJsonAsync(requestUrl, personalDetailsId);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("ManageUsers");
                }
                else
                {
                    return RedirectToAction("ManageUsers");
                };
            }

        }

        [HttpGet]
        public async Task<IActionResult> ManageOfficers(string username, int role)
        {

            username = HttpContext.Session.GetString("user");
            if (username == null)
            {
                HttpContext.Session.SetString(SessionUserName, username);
            }



            if (username == null && role != 1)
            {
                return RedirectToAction("Login", "Accounts");
            }
            UsersVM users = new UsersVM();
            var requestUrl = $"{BaseUrl}{apiUri}/GetAllUsers";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    users.UserDetailDTOs = await response.Content.ReadAsAsync<IEnumerable<UserDetailDTO>>();
                    users.UserDetailDTOs = users.UserDetailDTOs.Where(x => x.RoleId != 2);

                    var totals = await Totals();
                    users.CrimesCount = totals.CrimesCount;
                    users.Licenses = totals.Licenses;
                    users.CrimesToday = totals.CrimesToday;
                    users.VehiclesRegistered = totals.VehiclesRegistered;


                };

            };

            return View(users);
        }

        public async Task<IActionResult> DeleteUser(long personId)
        {
            ResultHandler resultHandler = new ResultHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/DeleteById?personId={personId}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.DeleteAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    resultHandler = await response.Content.ReadAsAsync<ResultHandler>();
                    return RedirectToAction("ManageUsers");
                }
                else
                {
                    return RedirectToAction("Edit", "Accounts", new ResultHandler { IsErrorOccured = true, Result = personId });
                }

            };
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int personalDetailsId, ResultHandler resultHandler = null)
        {
            var role = HttpContext.Session.GetInt32(SessionKeyRole);
            var username = HttpContext.Session.GetString(SessionUserName);
            if (username == null)
            {
                HttpContext.Session.SetString(SessionUserName, username);
            }

            if (resultHandler.Result != null)
            {
                personalDetailsId = (int)resultHandler.Result;
            }

            if (username == null && role != 1)
            {
                return RedirectToAction("Login", "Accounts");
            }
            UserDTO user = new UserDTO();
            var requestUrl = $"{BaseUrl}{apiUri}/GetUserById?personId={personalDetailsId}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    user = await response.Content.ReadAsAsync<UserDTO>();

                };

            };

            IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
            IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
            IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
            user.Branches = branches;
            user.Positions = positions;
            user.Roles = roles;

            //True - means its  comming from edit and something went wrong
            if (resultHandler.IsErrorOccured == true)
            {
                user.IsErrorOccured = true;
                user.Message = "The system failed to delete the record, please contact inform Technical Administrator";
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(long personalDetailsId, UserDTO user)
        {

            ResultHandler resultHandler = new ResultHandler();

            if (ModelState.IsValid)
            {
                var requestUrl = $"{BaseUrl}{apiUri}/UpdateUser";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    user.RoleId = 2;


                    var result = await client.PostAsJsonAsync(client.BaseAddress, user);

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction("ManageUsers");
                    }
                    else
                    {

                        resultHandler = await result.Content.ReadAsAsync<ResultHandler>();
                        if (resultHandler.IsErrorOccured)
                        {
                            ModelState.AddModelError("", resultHandler.Message);
                        }

                        IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                        IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                        IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                        user.Branches = branches;
                        user.Positions = positions;
                        user.Roles = roles;

                        return View(user);
                    }

                };
            }
            else
            {
                IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                user.Branches = branches;
                user.Positions = positions;
                user.Roles = roles;
                return View(user);
            }


            return View(user);


        }
        [HttpGet]
        public async Task<IActionResult> EditOfficer(int personalDetailsId, ResultHandler resultHandler = null)
        {
            var role = HttpContext.Session.GetInt32(SessionKeyRole);
            var username = HttpContext.Session.GetString(SessionUserName);
            if (username == null)
            {
                HttpContext.Session.SetString(SessionUserName, username);
            }

            if (resultHandler.Result != null)
            {
                personalDetailsId = (int)resultHandler.Result;
            }

            if (username == null && role != 1)
            {
                return RedirectToAction("Login", "Accounts");
            }
            UserDTO user = new UserDTO();
            var requestUrl = $"{BaseUrl}{apiUri}/GetUserById?personId={personalDetailsId}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    user = await response.Content.ReadAsAsync<UserDTO>();

                };

            };

            IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
            IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
            IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
            user.Branches = branches;
            user.Positions = positions;
            user.Roles = roles;

            //True - means its  comming from edit and something went wrong
            if (resultHandler.IsErrorOccured == true)
            {
                user.IsErrorOccured = true;
                user.Message = "The system failed to delete the record, please contact inform Technical Administrator";
            }
            return View(user);
        }

        public async Task<IActionResult> EditOfficer(long personalDetailsId, UserDTO user)
        {

            ResultHandler resultHandler = new ResultHandler();

            if (ModelState.IsValid)
            {
                var requestUrl = $"{BaseUrl}{apiUri}/UpdateUser";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    user.RoleId = 1;


                    var result = await client.PostAsJsonAsync(client.BaseAddress, user);

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction("ManageOfficers");
                    }
                    else
                    {

                        resultHandler = await result.Content.ReadAsAsync<ResultHandler>();
                        if (resultHandler.IsErrorOccured)
                        {
                            ModelState.AddModelError("", resultHandler.Message);
                        }

                        IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                        IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                        IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                        user.Branches = branches;
                        user.Positions = positions;
                        user.Roles = roles;

                        return View(user);
                    }

                };
            }
            else
            {
                IEnumerable<Branch> branches = await StaticDataHandler.GetBranches(BaseUrl);
                IEnumerable<Positions> positions = await StaticDataHandler.GetPositions(BaseUrl);
                IEnumerable<Roles> roles = await StaticDataHandler.GetRoles(BaseUrl);
                user.Branches = branches;
                user.Positions = positions;
                user.Roles = roles;
                return View(user);
            }


            return View(user);


        }
    }
}