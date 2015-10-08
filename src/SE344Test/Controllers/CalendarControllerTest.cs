using System;
using SE344.Controllers;
using Xunit;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace SE344Test.Controllers
{
    public class CalendarControllerTest
    {
        [Fact]
        public void IndexDoesNotError()
        {
            var dut = new CalendarController();
            var result = dut.Index() as ViewResult;
        }
    }
}
