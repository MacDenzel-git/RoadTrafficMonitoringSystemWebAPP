using System;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;
 using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TMSWebPortal.Models;
using TMSWebPortal.Models.InsurancePool;
using System.Web;
using TMSWebPortal.DTO;
using System.Collections.Generic;

namespace TMSWebPortal.Controllers
{
    public class VehicleInsuranceController : Controller
    {
        private readonly IConfiguration _configuration;
        static readonly string apiUri = "DriverLicense";
        static readonly string apiUriVehicle = "VehicleInsurances";
        // GET: VehicleInsurance

        public string BaseUrl
        {
            get
            {
                return _configuration["EndPointURL"];
            }
        }

        public VehicleInsuranceController(IConfiguration configuration)
        {
            _configuration = configuration;
         

        }
        public async Task<IActionResult> GetVehicleDetails(GeneralFilter generalFilter)
        {

             var user = HttpContext.Session.GetString("user");
            
            var insurance = new VehicleInsurance();
            var outputHandler = new OutputHandler();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}/GetInsuranceDetails?loggedInUser={user}&RegistrationNumber={generalFilter.RegistrationNumber}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);



                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        outputHandler = await response.Content.ReadAsAsync<OutputHandler>();

                        if (!outputHandler.IsErrorOccured
                            )
                        {
                            insurance = JsonConvert.DeserializeObject<VehicleInsurance>(outputHandler.Result.ToString());

                        }
                    }
                    else
                    {
                        outputHandler = await response.Content.ReadAsAsync<OutputHandler>();
                        outputHandler.IsErrorOccured = true;
                    }



                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            if (outputHandler.IsErrorOccured)
            {
                VehicleInsuranceNotFound(outputHandler);
                return View(nameof(VehicleInsuranceNotFound));
            }

            return View(insurance);
        }
        public async Task<IActionResult> GetLicenseDetails(GeneralFilter generalFilter)
        {
            var driverLicense = new LicenseViewModel();
            var outputHandler = new OutputHandler();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}?loggedInUser=Denzel&LicenseNumber={generalFilter.LicenseNumber}&Vehicle={generalFilter.RegistrationNumber}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        outputHandler = await response.Content.ReadAsAsync<OutputHandler>();

                        if (!outputHandler.IsErrorOccured)
                        {
                            driverLicense = JsonConvert.DeserializeObject<LicenseViewModel>(outputHandler.Result.ToString());

                        }
                    };

                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            if (outputHandler.Result == null)
            {
                outputHandler.Identifier = generalFilter.LicenseNumber;
                LicenseNotFound(outputHandler);
                return View(nameof(LicenseNotFound));
            }

            return View(driverLicense);


        }
        public ActionResult VehicleInsuranceNotFound(OutputHandler output)
        {
            return View(output);
        }

        public ActionResult LicenseNotFound(OutputHandler outputHandler)
        {
            return View(outputHandler);
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<VehicleInsuranceDTO> driversLicense = new List<VehicleInsuranceDTO>();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUriVehicle}/GetAllVehicleInsurance";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        driversLicense = await response.Content.ReadAsAsync<IEnumerable<VehicleInsuranceDTO>>();
                    }
                    else
                    {
                        return View(response.RequestMessage);
                    }
                    return View(driversLicense);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> Details(long id)
        {
            var driverlicense = new VehicleInsuranceDTO();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUriVehicle}/GetVehicleInsurance?id={id}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ViewBag.Message = "Something went wrong, try again";
                        return View();
                    }
                    driverlicense = await response.Content.ReadAsAsync<VehicleInsuranceDTO>();
                }
                return View(driverlicense);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<ActionResult> Delete(long id)
        //{
        //    var driverlicense = new VehicleInsuranceDTO();
        //    try
        //    {
        //        var requestUrl = $"{BaseUrl}{apiUriVehicle}/GetVehicleInsurance?id={id}";
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(requestUrl);
        //            var response = await client.GetAsync(requestUrl);
        //            if (response.StatusCode != HttpStatusCode.OK)
        //            {
        //                ViewBag.Message = "Something went wrong, try again";
        //                return View();
        //            }
        //            driverlicense = await response.Content.ReadAsAsync<VehicleInsuranceDTO>();
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
        public async Task<ActionResult> Delete(long id)
        {
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUriVehicle}/DeleteVehicleInsurance?id={id}";
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

        public async Task<ActionResult> Edit(long id)
        {
            var driverlicense = new VehicleInsuranceDTO();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUriVehicle}/GetVehicleInsurance?id={id}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ViewBag.Message = "Something went wrong, try again";
                        return View();
                    }
                    driverlicense = await response.Content.ReadAsAsync<VehicleInsuranceDTO>();
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
        public async Task<ActionResult> Edit(VehicleInsuranceDTO vehicleInsuranceDTO)
        {
            //if(Convert.ToInt32(driverLicenseDTO.DateIssued.Year.ToString()) = 5)
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUriVehicle}/UpdateVehicleInsurance";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PutAsJsonAsync(requestUrl, vehicleInsuranceDTO);
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
        public IActionResult Create(string message)
        {
            if (message != null)
            {
                ViewBag.message = message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleInsuranceDTO vehicleInsuranceDTO)
        {
            if (vehicleInsuranceDTO.ExpiryDate > DateTime.UtcNow.AddYears(10))
            {
                string error = "Date cannot exceed 10 years";
                return RedirectToAction("Create", new { message = error });

            }
            if (vehicleInsuranceDTO.ExpiryDate < DateTime.UtcNow.AddHours(2))
            {
                string error = "Date cannot be before today";
                return RedirectToAction("Create",new { message = error });

            }
            if (vehicleInsuranceDTO.ExpiryDate < DateTime.UtcNow.AddMonths(6))
            {
                string error = "Minimum insurance is 6 Months";
                return RedirectToAction("Create", new { message = error });
            }

            vehicleInsuranceDTO.DateEffective = DateTime.UtcNow.AddHours(2);
            vehicleInsuranceDTO.IssuedBy = HttpContext.Session.GetString("user");
            if (vehicleInsuranceDTO == null)
            {
                return NotFound();
            }
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUriVehicle}/CreateVehicleInsurance";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                var response = await client.PostAsJsonAsync(requestUrl, vehicleInsuranceDTO);
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