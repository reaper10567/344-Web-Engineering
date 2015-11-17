using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SE344.Models;

namespace SE344.Services
{
    public interface IStockHistoryService
    {
        /// <summary>
        /// List stock identifiers that may have transactions associated with them.
        /// Anything not on this list will have zero transactions associated with it.
        /// </summary>
        IEnumerable<string> getKnownIdentifiers();

        /// <summary>
        /// Return all transactions
        /// </summary>
        IEnumerable<KeyValuePair<string, StockTransaction>> getTransactions();

        /// <summary>
        /// Return transactions filtered by identifier
        /// </summary>
        IEnumerable<StockTransaction> getTransactions(string identifier);

        void addTransaction(Stock stock, StockTransaction model);

        void clear();
    }

    public class StubStockHistoryService : IStockHistoryService
    {
        public IEnumerable<string> getKnownIdentifiers()
        {
            return this.getTransactions().ToList().Select(x => x.Key).Distinct();
        }

        public IEnumerable<StockTransaction> getTransactions(string identifier)
        {
            return this.getTransactions().ToList().FindAll(x => x.Key == identifier).Select(x => x.Value);
        }

        public IEnumerable<KeyValuePair<string, StockTransaction>> getTransactions()
        {
            List<KeyValuePair<string, StockTransaction>> retVal = new List<KeyValuePair<string, StockTransaction>>();

            retVal.Add(new KeyValuePair<string, StockTransaction>("YHOO", new StockTransaction(new DateTime(2015, 01, 01, 00, 00, 23), 24.06m, 5)));
            retVal.Add(new KeyValuePair<string, StockTransaction>("YHOO", new StockTransaction(new DateTime(2015, 01, 02, 00, 00, 23), 24.08m, -5)));
            retVal.Add(new KeyValuePair<string, StockTransaction>("TWTR", new StockTransaction(new DateTime(2015, 05, 01, 10, 50, 23), 1.06m, 10)));
            retVal.Add(new KeyValuePair<string, StockTransaction>("MSFT", new StockTransaction(new DateTime(2015, 01, 01, 00, 00, 00), 1.02m, 5)));
            retVal.Add(new KeyValuePair<string, StockTransaction>("MSFT", new StockTransaction(new DateTime(2015, 02, 01, 00, 00, 00), 1.12m, 5)));
            retVal.Add(new KeyValuePair<string, StockTransaction>("MSFT", new StockTransaction(new DateTime(2015, 03, 01, 00, 00, 00), 1.34m, 5)));
            retVal.Add(new KeyValuePair<string, StockTransaction>("MSFT", new StockTransaction(new DateTime(2015, 08, 01, 10, 50, 23), 2.00m, -10)));
            retVal.Add(new KeyValuePair<string, StockTransaction>("F", new StockTransaction(new DateTime(2015, 10, 05, 22, 50, 23), 12.00m, 5)));

            return retVal;
        }

        public void addTransaction(Stock stock, StockTransaction model) { }

        public void clear() { }
    }

    /*
    public class DbStockHistoryService : IStockInformationService
    {
        public List<StockTransaction> getTransactions(string identifier)
        {
            List<StockTransaction> retVal = new List<StockTransaction>();

            //???: is this actually a rule, or just a coincidence
            var FOUR_UPPERCASE_LETTERS = new Regex("[A-Z]{1,4}");
            if (FOUR_UPPERCASE_LETTERS.matches(identifier))
            {

            }
            else
            {
                throw new IllegalArgumentException("Invalid Ticker Symbol");
            }
        }

        public void addTransaction(Stock stock, StockTransaction model) { }
    }
    */
}
