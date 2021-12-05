using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSourceCurrencyApi.Client;
using OpenSourceCurrencyApi.Models;
using OpenSourceCurrencyApi.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace OpenSourceCurrencyApi.Repository
{
    public class CurrencyRepository: ICurrencyRepository
    {
        private readonly IGitHubClient _gitHubClient;

        public CurrencyRepository(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        /// <summary>
        /// return list of all currencies with their short and long form
        /// </summary>
        /// <returns></returns>
        public async Task<List<Currency>> GetAllCurrencies()
        {
            var result = await _gitHubClient.GetAllCurrencies();

            if(result == null)
            {
                throw new Exception("there is no result for all currencies endpoint");
            }

            JObject jsonObject = JObject.Parse(result);
            var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonObject.ToString());
            List<Currency> currencies = new List<Currency>();

            foreach (var item in response)
            {
                currencies.Add(new Currency { CurrencyShortName = item.Key, CurrencyFullName = item.Value });
            }
            return currencies;
        }

        /// <summary>
        /// returns the serilized string with comma seperated form with list of 
        /// currenices and their exhange rates 
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>

        public async Task<string> GetCurrencyComparisonFileResult(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentNullException("currency is null in GetCurrencyComparisonFileResult method");
            }

            CurrencyComparator result = await GetCurrencyComparisonResult(currency);

            IEnumerable<CurrencyPrice> currencyPriceList = result.CurrencyBasePriceList;

            var properties = typeof(CurrencyPrice).GetProperties().Select(x => x.Name).ToList();
            var headers = string.Join(",", properties);

            var sb = new StringBuilder();
            sb.AppendLine(headers);
            foreach (var data in currencyPriceList)
            {
                sb.AppendLine(data.CurrencyShortName + "," + data.ExchangePrice);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Retruns the comparison result of a currency with rest for the curent date
        /// </summary>
        /// <param name="currency">given currency</param>
        /// <returns>list of currenices and their exchaneg rates with date</returns>
        public async Task<CurrencyComparator> GetCurrencyComparisonResult(string currency){

            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentNullException("currency is null in GetCurrencyComparisonResult method");
            }

            var result = await _gitHubClient.GetCurrencyComparison(currency);            

            if (result == null) {
                throw new Exception("there is no comparson result present for the given currency");
            }
            else
            {          
                JObject jsonObject = JObject.Parse(result);                

                var pricesDict = JsonConvert.DeserializeObject<Dictionary<string,float>>(jsonObject[$"{currency}"].ToString()).ToList();
                

                //We are storing the price and currency key value pair in Custom Circular Linked List

                CircularLinkedList<CurrencyPrice> currencyPricesList = new CircularLinkedList<CurrencyPrice>();                

                foreach (var item in pricesDict)
                {
                    currencyPricesList.Add(new CurrencyPrice { CurrencyShortName = item.Key, ExchangePrice = item.Value });
                }

                //We are implementing the bubble sort based in the exhange price and currency name . 
                //sorting logic is explained in CurrencyPrice class , where Icomparable is implemented

                currencyPricesList.BubbleSort();
                
                return new CurrencyComparator { CurrencyBasePriceList = currencyPricesList , Date = (string)jsonObject["date"] };
            }

        }
        
    }
}
