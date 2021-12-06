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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetCurrencyComparisonResult(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                return BadRequest();
            }
            var currencyInput = currency.Trim().ToLower();
            var currenciesList = await _currencyRepository.GetAllCurrencies();

            if (currenciesList.FindIndex(x => x.CurrencyShortName == currencyInput) != -1)
            {
                var result = await _currencyRepository.GetCurrencyComparisonResult(currencyInput);
                if (result == null)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            else
            {
                return NotFound();
            }
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DownloadComparisonResult(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                return BadRequest();
            }
            var currencyInput = currency.Trim().ToLower();
            var currenciesList = await _currencyRepository.GetAllCurrencies();

            if (currenciesList.FindIndex(x => x.CurrencyShortName == currencyInput) != -1)
            {
                string fileName = $"{currencyInput}.csv";
                var fileResult = await _currencyRepository.GetCurrencyComparisonFileResult(currencyInput);

                if (string.IsNullOrEmpty(fileResult))
                {
                    return NoContent();
                }

                return File(new UTF8Encoding().GetBytes(fileResult), "text/csv", fileName);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
