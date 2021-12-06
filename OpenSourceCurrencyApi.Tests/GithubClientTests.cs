
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using OpenSourceCurrencyApi.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenSourceCurrencyApi.Tests
{
    public class GithubClientTests
    {

        [Fact]
        public async Task GetAllCurrencies_should_give_valid_response()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"[{ ""aud"": ""aud fullform"", ""usd"": ""usd full form""}]"),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies.min.json"),
            };

            var configurationMock = new Mock<IConfiguration>();

            var gitHubClient = new GitHubClient(httpClient, configurationMock.Object);

            var retrievedResult = await gitHubClient.GetAllCurrencies();

            Assert.NotNull(retrievedResult);
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());

        }


        [Fact]
        public async Task GetCurrencyComparison_should_give_valid_response()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"[{ ""date"": ""2021 - 12 - 06"",""eur"": { ""ada"": 0.837816} }]"),
            };
            
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);


            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/eur.json"),
            };

            var configurationMock = new Mock<IConfiguration>();

            var gitHubClient = new GitHubClient(httpClient, configurationMock.Object);

            var retrievedResult = await gitHubClient.GetCurrencyComparison("eur");

            Assert.NotNull(retrievedResult);
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());

        }
    }

}
