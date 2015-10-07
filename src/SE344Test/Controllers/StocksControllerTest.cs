using System;
using SE344.Controllers;
using Xunit;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace SE344Test.Controllers
{
    public class StocksControllerTest
    {

        [Fact]
        public async Task IndexDoesNotError()
        {
            var dut = new StocksController();
            var result = await dut.Index() as ViewResult;
            Assert.True(result.ViewData.ContainsKey("stocks"));
            // the specific value is not deterministic, but the value hould exist
        }

        [Theory]
        [InlineData("YHOO")]
        [InlineData("F")]
        [InlineData("1234")]
        [InlineData("\"")]
        [InlineData(";")]
        [InlineData("XXXX")]
        public async Task SearchStocksDoesNotError(string s)
        {
            var dut = new StocksController();
            var result = await dut.SearchStocks(s) as ViewResult;
            Assert.Equal(s, result.ViewData["symbol"]);
            Assert.True(result.ViewData.ContainsKey("ChartData"));
            Assert.True(result.ViewData.ContainsKey("LowHighData"));
        }
    }
}
