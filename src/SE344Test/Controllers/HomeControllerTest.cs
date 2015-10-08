using System;
using SE344.Controllers;
using Xunit;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace SE344Test.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public async Task IndexDoesNotError()
        {
            var dut = new HomeController();
            var result = await dut.Index() as ViewResult;
            Assert.True(result.ViewData.ContainsKey("Stocks"));
            // the specific value is not deterministic, but the value hould exist
        }
    }
}
