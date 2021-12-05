using OpenSourceCurrencyApi.Models;
using System.Threading.Tasks;

namespace OpenSourceCurrencyApi.Client
{
    /// <summary>
    /// Interface for git hub client
    /// </summary>
    public interface IGitHubClient
    {        
        Task<string> GetCurrencyComparison(string baseCurrency);
        Task<string> GetAllCurrencies();
    }
}