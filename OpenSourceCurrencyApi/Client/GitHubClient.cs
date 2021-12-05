using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSourceCurrencyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace OpenSourceCurrencyApi.Client
{
    /// <summary>
    /// Implements github client interface and creates client object to 
    /// handle http calls
    /// </summary>
    public class GitHubClient : IGitHubClient
    {

        private const string defaultTime = "latest/";
        private const string defaultEndPoint = "currencies/";

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public GitHubClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        /// <summary>
        /// gets all the currecies result
        /// </summary>
        /// <returns>all the currencies with short and long form</returns>
        public async Task<string> GetAllCurrencies()
        {

            string baseUrl = _configuration["BaseUrl"];
            var httpResponse = await _client.GetAsync($"{baseUrl}{defaultTime}currencies.min.json");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve all currenices");
            }

            return await httpResponse.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// gets the comparison for the currency given with rest for the current date
        /// </summary>
        /// <param name="baseCurrency">input curreny</param>
        /// <returns></returns>
        public async Task<string> GetCurrencyComparison(string baseCurrency)
        {
            if (string.IsNullOrEmpty(baseCurrency))
            {
                throw new ArgumentNullException(); ;
            }


            string baseUrl = _configuration["BaseUrl"];
            var httpResponse = await _client.GetAsync($"{baseUrl}{defaultTime}{defaultEndPoint}{baseCurrency}.min.json");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve comparison result");
            }

            return await httpResponse.Content.ReadAsStringAsync();
 
        }



    }
}
