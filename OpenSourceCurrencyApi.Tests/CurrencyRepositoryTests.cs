using Moq;
using Newtonsoft.Json;
using OpenSourceCurrencyApi.Client;
using OpenSourceCurrencyApi.Models;
using OpenSourceCurrencyApi.Repository;
using OpenSourceCurrencyApi.Structures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OpenSourceCurrencyApi.Tests
{
    public class CurrencyRepositoryTests
    {
        [Fact]
        public async Task GetAllCurrencies_should_return_null_when_response_empty()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            mockGitHubClient.Setup(x => x.GetAllCurrencies())
                .Returns(Task.FromResult(string.Empty));

            var repo = new CurrencyRepository(mockGitHubClient.Object);
            var result = await repo.GetAllCurrencies();

            Assert.Null(result);

        }

        [Fact]
        public async Task GetAllCurrencies_should_throw_jsonException_for_invalid_response()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            mockGitHubClient.Setup(x => x.GetAllCurrencies())
                .Returns(Task.FromResult("valid response"));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            await Assert.ThrowsAsync<JsonReaderException>(() => repo.GetAllCurrencies());
        }

        [Fact]
        public async Task GetAllCurrencies_should_throw_Exception_for_invalid_json()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();
            List<Currency> pseudoValidObject = new List<Currency>();
            pseudoValidObject.Add(new Currency { CurrencyFullName = "demo", CurrencyShortName = "demo" });
            var response = JsonConvert.SerializeObject(pseudoValidObject);
            mockGitHubClient.Setup(x => x.GetAllCurrencies())
                .Returns(Task.FromResult(response));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            await Assert.ThrowsAsync<JsonReaderException>(() => repo.GetAllCurrencies());
        }

        [Fact]
        public async Task GetAllCurrencies_should_give_response_for_valid_json()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();
            string jsonData = @"{
                               'ada': 'Cardano'
                               }";

            mockGitHubClient.Setup(x => x.GetAllCurrencies())
                    .Returns(Task.FromResult(jsonData));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            var response = await repo.GetAllCurrencies();

            Assert.Equal(typeof(List<Currency>), response.GetType());
        }

        [Fact]
        public async Task GetCurrencyComparisonResult_should_return_null_when_response_empty()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                .Returns(Task.FromResult(string.Empty));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            var result = await repo.GetCurrencyComparisonResult("demo");

            Assert.Null(result);

        }

        [Fact]
        public async Task GetCurrencyComparisonResult_should_throw_jsonException_for_invalid_response()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                .Returns(Task.FromResult("valid response"));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            await Assert.ThrowsAsync<JsonReaderException>(() => repo.GetCurrencyComparisonResult("demo"));
        }

        [Fact]
        public async Task GetCurrencyComparisonResult_should_raise_exception_for_invalid_json()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            CircularLinkedList<CurrencyPrice> currencyList = new CircularLinkedList<CurrencyPrice>();
            currencyList.Add(new CurrencyPrice { ExchangePrice = 1, CurrencyShortName = "demo" });
            CurrencyComparator validObject = new CurrencyComparator() { CurrencyBasePriceList = currencyList, Date = "01/01/2021" };

            var response = JsonConvert.SerializeObject(validObject);
            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                .Returns(Task.FromResult(response));


            var repo = new CurrencyRepository(mockGitHubClient.Object);

            await Assert.ThrowsAsync<NullReferenceException>(() => repo.GetCurrencyComparisonResult("demo"));
        }

        [Fact]
        public async Task GetCurrencyComparisonResult_should_give_response_for_valid_json()
        {

            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();
            string jsonData = @"{
                'date': '2021 - 12 - 06',
                'demo': {
                    'ada': 0.837816,
                    'aed': 4.153338}                 
                }";

            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                    .Returns(Task.FromResult(jsonData));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            var response = await repo.GetCurrencyComparisonResult("demo");

            Assert.Equal(typeof(CurrencyComparator), response.GetType());
        }

        [Fact]
        public async Task GetCurrencyComparisonFileResult_should_return_null_when_response_empty()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                .Returns(Task.FromResult(string.Empty));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            var result = await repo.GetCurrencyComparisonFileResult("demo");

            Assert.Null(result);

        }

        [Fact]
        public async Task GetCurrencyComparisonFileResult_should_throw_jsonException_for_invalid_response()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                .Returns(Task.FromResult("valid response"));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            await Assert.ThrowsAsync<JsonReaderException>(() => repo.GetCurrencyComparisonFileResult("demo"));
        }

        [Fact]
        public async Task GetCurrencyComparisonFileResult_should_raise_exception_for_invalid_json()
        {
            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();

            CircularLinkedList<CurrencyPrice> currencyList = new CircularLinkedList<CurrencyPrice>();
            currencyList.Add(new CurrencyPrice { ExchangePrice = 1, CurrencyShortName = "demo" });
            CurrencyComparator validObject = new CurrencyComparator() { CurrencyBasePriceList = currencyList, Date = "01/01/2021" };

            var response = JsonConvert.SerializeObject(validObject);
            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                .Returns(Task.FromResult(response));


            var repo = new CurrencyRepository(mockGitHubClient.Object);

            await Assert.ThrowsAsync<NullReferenceException>(() => repo.GetCurrencyComparisonFileResult("demo"));
        }

        [Fact]
        public async Task GetCurrencyComparisonFileResult_should_give_response_for_valid_json()
        {

            // Arrange
            var mockGitHubClient = new Mock<IGitHubClient>();
            string jsonData = @"{
                'date': '2021 - 12 - 06',
                'demo': {
                    'ada': 0.837816,
                    'aed': 4.153338}                 
                }";

            mockGitHubClient.Setup(x => x.GetCurrencyComparison("demo"))
                    .Returns(Task.FromResult(jsonData));

            var repo = new CurrencyRepository(mockGitHubClient.Object);

            var response = await repo.GetCurrencyComparisonFileResult("demo");

            Assert.Equal(typeof(string), response.GetType());
        }

    }
}
