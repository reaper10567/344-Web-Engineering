using System;
using SE344.Models;
using Xunit;

namespace SE344Test.Models
{
    public class StockTransactionTest
    {
        [Fact]
        public void TotalPriceShouldEqualSharesTimesPrice()
        {
            var trans = new StockTransaction(DateTime.Now, 2.5m, 2);
            Assert.Equal(5.0m, trans.TotalPrice);
        }
    }
}
