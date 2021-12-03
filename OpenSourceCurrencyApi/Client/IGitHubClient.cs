using OpenSourceCurrencyApi.Models;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi.Client
{
    public interface IGitHubClient
    {
        
        Task<string> GetCurrencyComparison(string baseCurrency);
    }
}