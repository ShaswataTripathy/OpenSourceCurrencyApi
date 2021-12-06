
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using OpenSourceCurrencyApi.Controllers;
using OpenSourceCurrencyApi.Models;
using OpenSourceCurrencyApi.Repository;
using OpenSourceCurrencyApi.Structures;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace OpenSourceCurrencyApi.Tests
{
    public class CurrencyControllerTests  
    {
        
        private CurrencyController currController;
        Mock<ICurrencyRepository> repositoyMock = new Mock<ICurrencyRepository>();
         
        public CurrencyControllerTests()
        {
            this.currController = new CurrencyController(repositoyMock.Object);
        }

        [Fact]
        public async Task GetCurrencyComparisonResult_should_throw_bad_request_for_null_input()
        {
            // Act
            var actualResult = await currController.GetCurrencyComparisonResult(null);
            
            //Assert
            BadRequestResult actual = actualResult as BadRequestResult;
            Assert.NotNull(actual);            
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task GetCurrencyComparisonResult_should_throw_bad_request_for_empty_input()
        {
            // Act
            var actualResult = await currController.GetCurrencyComparisonResult(string.Empty);

            //Assert
            BadRequestResult actual = actualResult as BadRequestResult;
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task GetCurrencyComparisonResult_should_give_reponse_for_valid_request()
        {
            // Arrange
            var mockRepository = new Mock<ICurrencyRepository>();
            CircularLinkedList<CurrencyPrice> currencyComparators = new CircularLinkedList<CurrencyPrice>();
            mockRepository.Setup(x =>  x.GetCurrencyComparisonResult("demo"))
                .Returns(Task.FromResult(new CurrencyComparator { Date = "01/01/2021", CurrencyBasePriceList = currencyComparators }));

            List<Currency> currencyList = new List<Currency> { new Currency { CurrencyFullName = "demo", CurrencyShortName = "demo" } };
            mockRepository.Setup(x => x.GetAllCurrencies())
                    .Returns(Task.FromResult(currencyList));

            var controller = new CurrencyController(mockRepository.Object);

            // Act
            var actualResult = await controller.GetCurrencyComparisonResult("demo");
                       

            //Assert
            OkObjectResult actual = actualResult as OkObjectResult;
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task DownloadComparisonResult_should_throw_bad_request_for_null_input()
        {
            // Act
            var actualResult = await currController.DownloadComparisonResult(null);

            //Assert
            BadRequestResult actual = actualResult as BadRequestResult;
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task DownloadComparisonResult_should_throw_bad_request_for_empty_input()
        {
            // Act
            var actualResult = await currController.DownloadComparisonResult(string.Empty);

            //Assert
            BadRequestResult actual = actualResult as BadRequestResult;
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
        }

        [Fact]
        public async Task DownloadComparisonResult_should_give_reponse_for_valid_request()
        {
            // Arrange
            var mockRepository = new Mock<ICurrencyRepository>();
            CircularLinkedList<CurrencyPrice> currencyComparators = new CircularLinkedList<CurrencyPrice>();
            mockRepository.Setup(x => x.GetCurrencyComparisonFileResult("demo"))
                .Returns(Task.FromResult(new CurrencyComparator { Date = "01/01/2021", CurrencyBasePriceList = currencyComparators }.ToString()));
            List<Currency> currencyList = new List<Currency> { new Currency { CurrencyFullName = "demo", CurrencyShortName = "demo" } };
            mockRepository.Setup(x => x.GetAllCurrencies())
                    .Returns(Task.FromResult(currencyList));

            var controller = new CurrencyController(mockRepository.Object);

            // Act
            var actualResult = await controller.DownloadComparisonResult("demo");

            //Assert

            Assert.IsType<FileContentResult>(actualResult);
            FileContentResult result = actualResult as FileContentResult;
            Assert.True(result.ContentType == "text/csv" && result.FileDownloadName == "demo.csv");
        }

        [Fact]
        public async Task DownloadComparisonResult_shoud_throw_not_found_for_invalid_input()
        {
            // Arrange
            var mockRepository = new Mock<ICurrencyRepository>();
            CircularLinkedList<CurrencyPrice> currencyComparators = new CircularLinkedList<CurrencyPrice>();
            mockRepository.Setup(x => x.GetCurrencyComparisonFileResult("demo"))
                .Returns(Task.FromResult(new CurrencyComparator { Date = "01/01/2021", CurrencyBasePriceList = currencyComparators }.ToString()));
            List<Currency> currencyList = new List<Currency> { new Currency { CurrencyFullName = "found", CurrencyShortName = "found" } };
            mockRepository.Setup(x => x.GetAllCurrencies())
                    .Returns(Task.FromResult(currencyList));

            var controller = new CurrencyController(mockRepository.Object);

            // Act
            var actualResult = await controller.DownloadComparisonResult("demo");

            //Assert
            NotFoundResult actual = actualResult as NotFoundResult;
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
        }

        [Fact]
        public async Task GetCurrencyComparisonResult_shoud_throw_not_found_for_invalid_input()
        {
            // Arrange
            var mockRepository = new Mock<ICurrencyRepository>();
            CircularLinkedList<CurrencyPrice> currencyComparators = new CircularLinkedList<CurrencyPrice>();
            mockRepository.Setup(x => x.GetCurrencyComparisonResult("demo"))
                .Returns(Task.FromResult(new CurrencyComparator { Date = "01/01/2021", CurrencyBasePriceList = currencyComparators }));
            List<Currency> currencyList = new List<Currency> { new Currency { CurrencyFullName = "found", CurrencyShortName = "found" } };
            mockRepository.Setup(x => x.GetAllCurrencies())
                    .Returns(Task.FromResult(currencyList));

            var controller = new CurrencyController(mockRepository.Object);

            // Act
            var actualResult = await controller.GetCurrencyComparisonResult("demo");

            //Assert
            NotFoundResult actual = actualResult as NotFoundResult;
            Assert.NotNull(actual);
            Assert.Equal((int)HttpStatusCode.NotFound, actual.StatusCode);
        }
    }


}
