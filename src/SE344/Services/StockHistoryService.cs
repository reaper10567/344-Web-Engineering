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
        List<string> getKnownIdentifiers();

        /// <summary>
        /// Return transactions filtered by identifier
        /// </summary>
        List<StockTransaction> getTransactions(string identifier);
    }

    public class StubStockHistoryService : IStockHistoryService
    {
        public List<string> getKnownIdentifiers()
        {
            var retVal = new List<string>();
            retVal.Add("YHOO");
            retVal.Add("TWTR");
            retVal.Add("MSFT");
            return retVal;
        }

        public List<StockTransaction> getTransactions(string identifier)
        {
            List<StockTransaction> retVal = new List<StockTransaction>();

            if ("YHOO" == identifier)
            {
                retVal.Add(new StockTransaction(new DateTime(2015, 01, 01, 00, 00, 23), 24.06m, 5));
                retVal.Add(new StockTransaction(new DateTime(2015, 01, 02, 00, 00, 23), 24.08m, -5));
            }
            else if ("TWTR" == identifier)
            {
                retVal.Add(new StockTransaction(new DateTime(2015, 05, 01, 10, 50, 23), 1.06m, 10));
            }
            else if ("MSFT" == identifier)
            {
                retVal.Add(new StockTransaction(new DateTime(2015, 01, 01, 00, 00, 00), 1.02m, 5));
                retVal.Add(new StockTransaction(new DateTime(2015, 02, 01, 00, 00, 00), 1.12m, 5));
                retVal.Add(new StockTransaction(new DateTime(2015, 03, 01, 00, 00, 00), 1.34m, 5));
                retVal.Add(new StockTransaction(new DateTime(2015, 08, 01, 10, 50, 23), 2.00m, -10));
            }
            else
            {
            }

            return retVal;
        }
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
    }
    */
}
