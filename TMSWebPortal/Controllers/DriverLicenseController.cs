using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TMSWebPortal.DTO;
using TMSWebPortal.Models;

namespace TMSWebPortal.Controllers
{
    public class DriverLicenseController : Controller
    {
        private readonly IConfiguration _configuration;
        static readonly string apiUri = "DriverLicense";

        public string BaseUrl
        {
            get
            {
                return _configuration["EndPointUrl"];
            }
        }
        public DriverLicenseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string message = null)
        {
            DriversLicenseVM driversLicenseVM = new DriversLicenseVM();
            IEnumerable<DriverLicenseDTO> driversLicense = new List<DriverLicenseDTO>();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}/GetAllDriversLicense";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        driversLicenseVM.DriverLicenses = await response.Content.ReadAsAsync<IEnumerable<DriverLicenseDTO>>();
                        driversLicenseVM.Message = message;
                    }
                    else
                    {
                        return View(response.RequestMessage);
                    }
                    return View(driversLicenseVM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var driverlicense = new DriverLicenseDTO();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}/GetDriversLicense?licenseNumber={id}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ViewBag.Message = "Something went wrong, try again";
                        return View();
                    }
                    driverlicense = await response.Content.ReadAsAsync<DriverLicenseDTO>();
                }
                return View(driverlicense);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<ActionResult> Delete(string id)
        //{
        //    var driverlicense = new DriverLicenseDTO();
        //    try
        //    {
        //        var requestUrl = $"{BaseUrl}{apiUri}/GetDriversLicense?licenseNumber={id}";
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(requestUrl);
        //            var response = await client.GetAsync(requestUrl);
        //            if (response.StatusCode != HttpStatusCode.OK)
        //            {
        //                ViewBag.Message = "Something went wrong, try again";
        //                return View();
        //            }
        //            driverlicense = await response.Content.ReadAsAsync<DriverLicenseDTO>();
        //        }
        //        return View(driverlicense);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/DeleteDriversLicense?driverLicense={id}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                var response = await client.DeleteAsync(requestUrl);
                outputHandler = await response.Content.ReadAsAsync<OutputHandler>();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (outputHandler.IsErrorOccured)
                    {
                        ViewBag.Message = outputHandler.Message;
                        return View();
                    }
                }

                return RedirectToAction("Index", new { outputHandler });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            var driverlicense = new DriverLicenseDTO();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}/GetDriversLicense?licenseNumber={id}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ViewBag.Message = "Something went wrong, try again";
                        return View();
                    }
                    driverlicense = await response.Content.ReadAsAsync<DriverLicenseDTO>();
                }
                return View(driverlicense);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DriverLicenseDTO driverLicenseDTO)
        {
            //if(Convert.ToInt32(driverLicenseDTO.DateIssued.Year.ToString()) = 5)
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/UpdateDriversLicense";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PutAsJsonAsync(requestUrl, driverLicenseDTO);
                outputHandler = await response.Content.ReadAsAsync<OutputHandler>();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (outputHandler.IsErrorOccured)
                    {
                        ViewBag.Message = outputHandler.Message;
                        return View();
                    }
                }
            }
            return RedirectToAction("Index", new { outputHandler.Message });
        }

        public IActionResult Duplicate()
        {
            
            return View(new OutputHandler { Message = "This Individual already has an active license"});
        }

        public async Task<OutputHandler>  LicenseExist(int id)
        {
            var driverlicense = new DriverLicenseDTO();
         
                var requestUrl = $"{BaseUrl}{apiUri}/GetDriversLicenseById?personId={id}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {

                }
                else
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return new OutputHandler { IsErrorOccured = true, Message = "Something went wrong, try again" };
                    }
                }
                 
                driverlicense = await response.Content.ReadAsAsync<DriverLicenseDTO>();
                if (driverlicense != null)
                {
                    if (driverlicense.ExpiryDate < DateTime.UtcNow.AddHours(2))
                    {
                        return new OutputHandler { IsErrorOccured = false, Result = driverlicense,Message = "false" };
                    }
                        return new OutputHandler { IsErrorOccured = false, Result = true };
                }
                else
                {
                    return new OutputHandler { IsErrorOccured = false, Result = false };
                }
            }

           
        }
        public async Task<IActionResult> Create(int personalDetailsId,string firstname,string Lastname)
        {
            DateTime firstIssue = new DateTime();
            var output = await LicenseExist(personalDetailsId);
            if (output.IsErrorOccured == false)
            {
                if (output.Result.Equals(true))
                {
                    return RedirectToAction("Duplicate");
                }
                else
                {
                    if (output.Result !=null && !output.Result.Equals(false))
                    {
                        var result = (DriverLicenseDTO)output.Result;
                        firstIssue = result.FirstIssue;
                    }
                    else
                    {
                        firstIssue = DateTime.UtcNow.AddHours(2);
                    }
                    
                }
            }

            string personInfo = $"{firstname} {Lastname}-{personalDetailsId}";
            if (personInfo != null)
            {
                DriverLicenseDTO driverLicenseDTO = new DriverLicenseDTO
                {
                    PersonId = personalDetailsId,
                    PersonInfo = personInfo,
                    DateIssued = DateTime.UtcNow.AddHours(2),
                    FirstIssue = firstIssue
                    

                };
                return View(driverLicenseDTO);
            }
            else
            {
                return View(new DriverLicenseDTO { PersonId = 0 });
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> Create(DriverLicenseDTO driverLicenseDTO)
        {

                if (!ModelState.IsValid)
                            {
                                return View(new DriverLicenseDTO { PersonId = driverLicenseDTO.PersonId });
                            }
            driverLicenseDTO.DateIssued = DateTime.UtcNow.AddHours(2);
            driverLicenseDTO.ExpiryDate = driverLicenseDTO.DateIssued.AddYears(5);
            if (driverLicenseDTO == null)
            {
                return NotFound();
            }
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/CreateDriversLicense";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                var response = await client.PostAsJsonAsync(requestUrl, driverLicenseDTO);
                outputHandler = await response.Content.ReadAsAsync<OutputHandler>();
                if (outputHandler.IsErrorOccured)
                {
                    ViewBag.Message = outputHandler.Message;
                    return View();
                }
            }
            return RedirectToAction("Index", new { outputHandler.Message });
        }
    }
}