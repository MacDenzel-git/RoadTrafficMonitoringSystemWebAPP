 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TMSWebPortal.DTO
{
    public class StaticDataHandler
    {
        public static async Task<IEnumerable<Branch>> GetBranches(string baseUrl)
        {
            var requestUrl = $"{baseUrl}Branch/GetBranches";
            IEnumerable<Branch> branches = new List<Branch>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    branches = await response.Content.ReadAsAsync<IEnumerable<Branch>>();
                };

            };

            return branches;
        }
        public static async Task<IEnumerable<Positions>> GetPositions(string baseUrl)
        {
            var requestUrl = $"{baseUrl}Positions/GetAllPositions";
            IEnumerable<Positions> positions = new List<Positions>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    positions = await response.Content.ReadAsAsync<IEnumerable<Positions>>();
                };

            };

            return positions;
        }

        public static async Task<IEnumerable<Roles>> GetRoles(string baseUrl)
        {
            var requestUrl = $"{baseUrl}Roles/GetAllRoles";
            IEnumerable<Roles> roles = new List<Roles>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    roles = await response.Content.ReadAsAsync<IEnumerable<Roles>>();
                };

            };

            return roles;
        }


        public static async Task<IEnumerable<Crimes>> GetCrimes(string baseUrl)
        {
            var requestUrl = $"{baseUrl}Crimes/GetCrimes";
            IEnumerable<Crimes> roles = new List<Crimes>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    roles = await response.Content.ReadAsAsync<IEnumerable<Crimes>>();
                };

            };

            return roles;
        }

 
    }
}
