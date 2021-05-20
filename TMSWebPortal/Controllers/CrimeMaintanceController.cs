using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TMSWebPortal.DTO;
using TMSWebPortal.Models;

namespace TMSWebPortal.Controllers
{
    public class CrimeMaintanceController : Controller
    {
        static readonly string apiUri = "CrimeMaintanaince";
        readonly IConfiguration _configuration;
        public CrimeMaintanceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: VehicleInsurance

        public string BaseUrl
        {
            get
            {
                return _configuration["EndPointURL"];
            }
        }

        // GET: CrimeMaintance
        public async Task<ActionResult> Index()
        {
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}/GetAll";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    IEnumerable<CrimeMantainainceDTO> crimes = new List<CrimeMantainainceDTO>();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        crimes = await response.Content.ReadAsAsync<IEnumerable<CrimeMantainainceDTO>>();
                    }
                    else
                    {
                        return View();
                    }
                    return View(crimes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: CrimeMaintance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CrimeMaintance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CrimeMaintance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CrimeMantainainceDTO crime)
        {
          
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/Create";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                var response = await client.PostAsJsonAsync(requestUrl, crime);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index", new { outputHandler.Message });
                }
                outputHandler = await response.Content.ReadAsAsync<OutputHandler>();
                if (outputHandler.IsErrorOccured)
                {
                    ViewBag.Message = outputHandler.Message;
                    return View();
                }
            }
            return RedirectToAction("Index", new { outputHandler.Message });
        }

        // GET: CrimeMaintance/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var crime = new CrimeMantainainceDTO();
            try
            {
                var requestUrl = $"{BaseUrl}{apiUri}/GetCrime?id={id}";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    var response = await client.GetAsync(requestUrl);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ViewBag.Message = "Something went wrong, try again";
                        return View();
                    }
                    crime = await response.Content.ReadAsAsync<CrimeMantainainceDTO>();
                }
                return View(crime);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: CrimeMaintance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CrimeMantainainceDTO collection)
        {
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/edit";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.PutAsJsonAsync(requestUrl, collection);
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

        //GET: CrimeMaintance/Delete/5
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var crime = new CrimeMantainainceDTO();
        //    try
        //    {
        //        var requestUrl = $"{BaseUrl}{apiUri}/GetCrime?id={id}";
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(requestUrl);
        //            var response = await client.GetAsync(requestUrl);
        //            if (response.StatusCode != HttpStatusCode.OK)
        //            {
        //                ViewBag.Message = "Something went wrong, try again";
        //                return View();
        //            }
        //            crime = await response.Content.ReadAsAsync<CrimeMantainainceDTO>();
        //        }
        //        return View(crime);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        // POST: CrimeMaintance/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var outputHandler = new OutputHandler();
            var requestUrl = $"{BaseUrl}{apiUri}/Delete?crime={id}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.DeleteAsync(requestUrl);
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
    }
}