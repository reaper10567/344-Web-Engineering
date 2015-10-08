using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using SE344.Services;
using SE344.Models;
using Xunit;
using Xunit.Abstractions;

namespace SE344Test.Services
{
    public class YahooStockInformationServiceTest
    {
        private readonly YahooStockInformationService _stockInfo;
        private readonly ITestOutputHelper _output;

        public YahooStockInformationServiceTest(ITestOutputHelper output)
        {
            _stockInfo = new YahooStockInformationService();
            _output = output;
        }

        [Theory]
        [InlineData("YHOO")]
        [InlineData("TWTR")]
        [InlineData("MSFT")]
        [InlineData("GOOG")]
        [InlineData("F")]
        public async void GetQuoteAsyncShouldNotErrorForValidId(string id)
        {
            var searchViewModel = await _stockInfo.GetQuoteAsync(new Stock(id));
            _output.WriteLine("Current price of {0} is {1}", searchViewModel.Identifier, searchViewModel.CurrentPrice);
            _output.WriteLine("Day's range is {0} to {1}", searchViewModel.DaysLow, searchViewModel.DaysHigh);
            Assert.NotNull(searchViewModel.CurrentPrice);
            Assert.InRange((decimal) searchViewModel.CurrentPrice, 0, 10000m);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("\"")]
        [InlineData(";")]
        [InlineData("XXXX")]
        public async void GetQuoteAsyncShouldReturnNullValuesForInvalidId(string id)
        {
            var searchViewModel = await _stockInfo.GetQuoteAsync(new Stock(id));
            Assert.Null(searchViewModel.CurrentPrice);
        }

        [Fact]
        public async void GetHistoryInfoAsyncShouldMatchPastFetches()
        {
            var result = JArray.Parse(await _stockInfo.GetHistoryInfoAsync("YHOO", new DateTime(2015,10,01,00,00,00), new DateTime(2015,10,02,00,00,00)));
            var data = result.Single(x => ((string) x["Date"]) == "2015-10-01");
            Assert.Equal("28.950001", (string) data["Open"]);
            Assert.Equal("29.00", (string) data["High"]);
            Assert.Equal("28.440001", (string) data["Low"]);
            Assert.Equal("28.91", (string) data["Close"]);
        }

        [Theory]
        [InlineData("YHOO")]
        [InlineData("TWTR")]
        [InlineData("MSFT")]
        [InlineData("GOOG")]
        [InlineData("F")]
        public async void GetHistoryInfoAsyncShouldNotErrorForValidId(string id)
        {
            var result = JArray.Parse(await _stockInfo.GetHistoryInfoAsync(id, DateTime.Now.AddDays(-7), DateTime.Now));
            Assert.InRange((decimal) result[0]["Close"], 0, 10000m);
            Assert.InRange((decimal) result[0]["Low"], 0, 10000m);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("\"")]
        [InlineData(";")]
        [InlineData("XXXX")]
        public async void GetHistoryInfoAsyncShouldErrorValuesForInvalidId(string id)
        {
            await Assert.ThrowsAsync<UnknownTickerSymbolException>(async () => await _stockInfo.GetHistoryInfoAsync(id, DateTime.Now.AddDays(-7), DateTime.Now));
       }
    }
}
