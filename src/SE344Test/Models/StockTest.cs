using System;
using SE344.Models;
using Xunit;

namespace SE344Test.Models
{
    public class StockTest
    {
        private readonly Stock _stock;
        private readonly StockTransaction _purchase1, _purchase2, _sale1;

        public StockTest()
        {
            _stock = new Stock("MSFT", 5.5m);
            _purchase1 = new StockTransaction(DateTime.Now, 4.0m, 7);
            _purchase2 = new StockTransaction(DateTime.Now.AddDays(-1), 7.5m, 2);
            _sale1 = new StockTransaction(DateTime.Now, 5.0m, -1);
        }

        [Fact]
        public void TotalSharesOwnedShouldEqualSharesFromAllTransactions()
        {
            _stock.Transactions.Add(_purchase1);
            _stock.Transactions.Add(_purchase2);
            _stock.Transactions.Add(_sale1);

            Assert.Equal(8, _stock.CurrentlyOwned);
        }

        [Fact]
        public void ProfitShouldEqualCostFromAllTransactions()
        {
            _stock.Transactions.Add(_purchase2);
            _stock.Transactions.Add(_sale1);

            Assert.Equal(10, _stock.Profit);
        }
    }
}
