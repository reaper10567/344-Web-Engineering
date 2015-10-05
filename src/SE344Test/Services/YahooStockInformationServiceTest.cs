using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using SE344.Services;
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
        public async void CurrentPriceShouldNotErrorForValidId(string id)
        {
            var price = await _stockInfo.CurrentPrice(id);
            _output.WriteLine("Current price of {0} is {1}", id, price);
            Assert.InRange(price, 0, 10000m);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("\"")]
        [InlineData(";")]
        [InlineData("XXXX")]
        public async void CurrentPriceShouldErrorForInvalidId(string id)
        {
            await Assert.ThrowsAsync<FormatException>(async () => await _stockInfo.CurrentPrice(id));
        }

        [Theory]
        [InlineData("YHOO")]
        [InlineData("TWTR")]
        [InlineData("MSFT")]
        [InlineData("GOOG")]
        [InlineData("F")]
        public async void GetQuoteAsyncShouldNotErrorForValidId(string id)
        {
            var searchViewModel = await _stockInfo.GetQuoteAsync(id);
            _output.WriteLine("Current price of {0} is {1}", searchViewModel.Symbol, searchViewModel.CurrentPrice);
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
            var searchViewModel = await _stockInfo.GetQuoteAsync(id);
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
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _stockInfo.GetHistoryInfoAsync(id, DateTime.Now.AddDays(-7), DateTime.Now));
       }
    }
}
