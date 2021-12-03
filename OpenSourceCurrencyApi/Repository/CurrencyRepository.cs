using OpenSourceCurrencyApi.Client;

namespace OpenSourceCurrencyApi.Repository
{
    public class CurrencyRepository
    {
        private readonly IGitHubClient _gitHubClient;

        public CurrencyRepository(IGitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }
        public void GetCurrencyComparisonResult(string currency){

            
        }
    }
}
