using OpenSourceCurrencyApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi.Repository
{
    /// <summary>
    /// Interface for Repository layer which will act bridge between client and controller
    /// </summary>
    public interface ICurrencyRepository
    {
        Task<CurrencyComparator> GetCurrencyComparisonResult(string currency);
        Task<List<Currency>> GetAllCurrencies();
        Task<string> GetCurrencyComparisonFileResult(string currency);
    }
}
