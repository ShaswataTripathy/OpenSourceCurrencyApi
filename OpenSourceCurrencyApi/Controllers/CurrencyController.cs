using Microsoft.AspNetCore.Mvc;
using OpenSourceCurrencyApi.Client;
using OpenSourceCurrencyApi.Repository;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        [HttpGet("GetCurrencyComparison/{currency}")]
        public async Task<IActionResult> GetCurrencyComparisonResult(string currency)
        {
            var result = await _currencyRepository.GetCurrencyComparisonResult(currency);

            return Ok(result);
        }

        [HttpGet("GetAllCurrencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var result = await _currencyRepository.GetAllCurrencies();

            return Ok(result);
        }
    }
}
