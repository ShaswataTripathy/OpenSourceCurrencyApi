using OpenSourceCurrencyApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi.Repository
{
    public interface ICurrencyRepository
    {
        Task<CurrencyComparator> GetCurrencyComparisonResult(string currency);
        Task<List<Currency>> GetAllCurrencies();
    }
}
