using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OpenSourceCurrencyApi.Client;
using OpenSourceCurrencyApi.Models;
using OpenSourceCurrencyApi.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OpenSourceCurrencyApi.Controllers
{
    [ApiController]
    [Route("currency")]
    [EnableCors("CorsApi")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        [HttpGet("comparison/{currency}")]
        public async Task<IActionResult> GetCurrencyComparisonResult(string currency)
        {
            var result = await _currencyRepository.GetCurrencyComparisonResult(currency);

            return Ok(result);
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var result = await _currencyRepository.GetAllCurrencies();

            return Ok(result);
        }

        [HttpGet("comparison/download/{currency}")]
        public async Task<FileResult> DownloadComparisonResult(string currency)
        {
            CurrencyComparator result = await _currencyRepository.GetCurrencyComparisonResult(currency);
            
            string fileName = $"{result.Date}_{currency}";
            

            
            IEnumerable<CurrencyPrice> currencyPriceList = result.CurrencyBasePriceList;

            var properties = typeof(CurrencyPrice).GetProperties().Select(x=> x.Name).ToList();
            var headers = string.Join(",", properties);

            var sb = new StringBuilder();
            sb.AppendLine(headers);
            foreach (var data in currencyPriceList)
            {
                sb.AppendLine(data.CurrencyShortName + "," + data.ExchangePrice);
            }

            return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", fileName);
        }
    }
}
