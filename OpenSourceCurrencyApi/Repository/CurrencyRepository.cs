using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSourceCurrencyApi.Client;
using OpenSourceCurrencyApi.Models;
using OpenSourceCurrencyApi.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace OpenSourceCurrencyApi.Repository
{
    public class CurrencyRepository: ICurrencyRepository
    {
        private readonly IGitHubClient _gitHubClient;

        public CurrencyRepository(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            var result = await _gitHubClient.GetAllCurrencies();
            JObject jsonObject = JObject.Parse(result);
            var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonObject.ToString());
            List<Currency> currencies = new List<Currency>();

            foreach (var item in response)
            {
                currencies.Add(new Currency { CurrencyShortName = item.Key, CurrencyFullName = item.Value });
            }
            return currencies;
        }

        public async Task<CurrencyComparator> GetCurrencyComparisonResult(string currency){
            var result = await _gitHubClient.GetCurrencyComparison(currency);
            

            if (result == null) { 
                return null;
            }
            else
            {               

                JObject jsonObject = JObject.Parse(result);                

                var pricesDict = JsonConvert.DeserializeObject<Dictionary<string,float>>(jsonObject[$"{currency}"].ToString()).ToList();
                
                CircularLinkedList<CurrencyPrice> currencyPricesList = new CircularLinkedList<CurrencyPrice>();                

                foreach (var item in pricesDict)
                {
                    currencyPricesList.Add(new CurrencyPrice { CurrencyShortName = item.Key, ExchangePrice = item.Value });
                }

                currencyPricesList.BubbleSort();
                
                return new CurrencyComparator { CurrencyBasePriceList = currencyPricesList , Date = (string)jsonObject["date"] };
            }

            


        }

        
    }
}
