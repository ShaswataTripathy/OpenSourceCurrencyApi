using Microsoft.AspNetCore.Mvc;
using OpenSourceCurrencyApi.Client;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : Controller
    {
        private readonly IGitHubClient _gitHubClient;

        public CurrencyController(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        [HttpGet("GetCurrencyComparison/{currency}")]
        public async Task<IActionResult> GetCurrencyComparisonResult(string currency)
        {
            var company = await _gitHubClient.GetCurrencyComparison(currency);

            return Ok(company);
        }
    }
}
