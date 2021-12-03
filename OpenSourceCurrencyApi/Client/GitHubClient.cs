using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSourceCurrencyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi.Client
{
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

        public async Task<string> GetCurrencyComparison(string currency)
        {
            string baseUrl = _configuration["BaseUrl"];
            var httpResponse = await _client.GetAsync($"{baseUrl}{defaultTime}{defaultEndPoint}{currency}.min.json");

            var content = await httpResponse.Content.ReadAsStringAsync();


            //JObject result = JObject.Parse(content);

            //string date = (string)result["date"];

            //var pricesDict = JsonConvert.DeserializeObject<Dictionary<string, float>>(result[$"{currency}"].ToString());

            //var pricesList = pricesDict.Select(x => new CurrencyPrice { CurrencyShortName = x.Key, ExchangePrice = x.Value }).ToList();

            //new CurrencyComparator { CurrencyBasePriceList = pricesList, Date = date };
            return content;
        }
    }
}
