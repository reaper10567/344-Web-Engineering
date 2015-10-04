using System;
using SE344.Models;
using SE344.Services;
using Xunit;

namespace SE344Test.Services
{
    public class YahooStockInformationServiceTest
    {
        private readonly YahooStockInformationService _stockInfo;

        public YahooStockInformationServiceTest()
        {
            _stockInfo = new YahooStockInformationService();
        }

        [Theory]
        [InlineData("YHOO")]
        [InlineData("TWTR")]
        [InlineData("MSFT")]
        [InlineData("GOOG")]
        [InlineData("F")]
        public void CurrentPriceShouldNotErrorForValidId(string id)
        {
            Assert.DoesNotThrow(() => _stockInfo.currentPrice(id));
            Assert.InRange(0, 10000d, _stockInfo.currentPrice(id));
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("\"")]
        [InlineData(";")]
        public void CurrentPriceShouldErrorForInvalidId(string id)
        {
            Assert.Throws<FormatException>(() => _stockInfo.currentPrice(id));
        }

        [Theory]
        [InlineData("XXXX")]
        public void CurrentPriceShouldErrorForUnusedId(string id)
        {
            //TODO: Provide more meaningful error than "number format exception"
            Assert.Throws<FormatException>(() => _stockInfo.currentPrice(id));
        }
    }
}
