using System;
using System.Collections.Generic;
using System.Linq;

namespace SE344.Models
{
    /// <summary>
    /// Information about a particular stock
    /// </summary>
    public class Stock
    {
        public Stock(string identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// The stock's four-letter identifier
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// The current price of this stock
        /// </summary>
        public decimal? CurrentPrice { get; set; }

        public decimal? DaysHigh { get; set; }
        public decimal? DaysLow { get; set; }
        public decimal? YearsHigh { get; set; }
        public decimal? YearsLow { get; set; }

        /// <summary>
        /// A set of transactions made by the user relating to this stock.
        /// 
        /// List is mutable. To change this field's value, mutate the list returned by the accessor.
        /// </summary>
        public List<StockTransaction> Transactions { get; } = new List<StockTransaction>();

        /// <summary>
        /// A user-provided note about the stock.
        /// </summary>
        public string Note { get; set; } = "";

        /// <summary>
        /// The number of shares currently owned by the user
        /// </summary>
        public int CurrentlyOwned
        {
            get
            {
                return Transactions.Select(x => x.NumShares).Sum();
            }
        }

        /// <summary>
        /// The profit made by this user
        /// </summary>
        public decimal Profit
        {
            get 
            {
                return Transactions.Select(x => x.TotalPrice).Sum();
            }
        }
    }
}
