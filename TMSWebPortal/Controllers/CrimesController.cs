using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TMSWebPortal.DTO;
using TMSWebPortal.Models;
using TMSWebPortal.Models.TMS;

namespace TMSWebPortal.Controllers
{
    public class CrimesController : Controller
    {
        private readonly IConfiguration _configuration;
        static string apiUri = "Crimes";
        // GET: VehicleInsurance

        public CrimesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string BaseUrl
        {
            get
            {
                return _configuration["EndPointURL"];
            }
        }
        public async Task<IActionResult> GetCrimes(string id, string checkType)
        {
            var trafficMonitorTransaction = new List<TrafficMonitorTransactions>();
            try
            {//checktype is to identify where request is coming from L for license and V for vehicle check
                var requestUrl = $"{BaseUrl}{apiUri}/GetRegistrationCharges?registrationNumber={id}&checkType={checkType}";
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        trafficMonitorTransaction = await response.Content.ReadAsAsync<List<TrafficMonitorTransactions>>();

                    };

                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return View(trafficMonitorTransaction);
        }

        public async Task<IActionResult> GetTransactions(DateTime startDate, DateTime endDate)
        {
            var start = new DateTime();
            bool isDateRangeProvided = false;
            if (startDate.Year != 0001)
            {
                start = startDate;
                
            }
            var end = new DateTime();
            if (endDate.Year != 0001)
            {
                 end = startDate;
                
            }
            if (start.Year != 0001 && end.Year != 0001)
            {
                isDateRangeProvided = true;
            }
            var trafficMonitorTransaction = new List<TrafficMonitorTransactions>();
            try
            {//checktype is to identify where request is coming from L for license and V for vehicle check
                var requestUrl = $"{BaseUrl}{apiUri}/GetTransactions?start={start}&end={end}&IsDateRangeProvided={isDateRangeProvided}";
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        trafficMonitorTransaction = await response.Content.ReadAsAsync<List<TrafficMonitorTransactions>>();

                    };

                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return View(trafficMonitorTransaction);
        }
        public async Task<IActionResult> ChargeForSpecific(GeneralFilter generalFilter)
        {
            var crime = new CrimeDTO();
            crime.CrimeName = generalFilter.CrimeName;
            crime.VehicleRegistrationNumber = generalFilter.RegistrationNumber;
            crime.LicenseNumber = generalFilter.LicenseNumber;
            crime.LoggedInUser  = HttpContext.Session.GetString("user");

            var outputHandler = new OutputHandler();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}/Charged";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.PostAsJsonAsync(requestUrl, crime);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        outputHandler = await response.Content.ReadAsAsync<OutputHandler>();

                        if (!outputHandler.IsErrorOccured)
                        {
                            crime = JsonConvert.DeserializeObject<CrimeDTO>(outputHandler.Result.ToString());

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
                outputHandler.IsErrorOccured = true;
                outputHandler.Message = "Something went wrong";
                return View();
            }

            return View(crime);

        }
        }
 
}