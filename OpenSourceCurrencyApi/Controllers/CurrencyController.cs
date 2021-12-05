using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OpenSourceCurrencyApi.Client;
using OpenSourceCurrencyApi.Models;
using OpenSourceCurrencyApi.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyComparator))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrencyComparisonResult(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                return BadRequest();
            }

            var result = await _currencyRepository.GetCurrencyComparisonResult(currency);
            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("currencies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Currency>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var result = await _currencyRepository.GetAllCurrencies();

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("comparison/download/{currency}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DownloadComparisonResult(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                return BadRequest();
            }
            string fileName = $"{currency}.csv";
            var fileResult = await _currencyRepository.GetCurrencyComparisonFileResult(currency);

            if (string.IsNullOrEmpty(fileResult))
            {
                return NotFound();
            }

            return File(new UTF8Encoding().GetBytes(fileResult), "text/csv", fileName);
        }
    }
}
