using System;
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
        public async void CurrentPriceShouldErrorForInvalidId(string id)
        {
            await Assert.ThrowsAsync<FormatException>(async () => await _stockInfo.CurrentPrice(id));
        }

        [Theory]
        [InlineData("XXXX")]
        public async void CurrentPriceShouldErrorForUnusedId(string id)
        {
            //TODO: Provide more meaningful error than "number format exception"
            await Assert.ThrowsAsync<FormatException>(async () => await _stockInfo.CurrentPrice(id));
        }
    }
}
