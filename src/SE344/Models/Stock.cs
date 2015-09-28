using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SE344.Models
{
    /// <summary>
    /// Information about a particular stock
    /// </summary>
    public class Stock
    {
        public Stock(string identifier, Currency currentPrice)
        {
            this.identifier = identifier;
            this.currentPrice = currentPrice;
            this.note = "";
            this.transactions = new List<StockTransaction>();
        }

        private readonly string identifier;
        private Currency currentPrice;
        private string note;
        private List<StockTransaction> transactions;

        /// <summary>
        /// The stock's four-letter identifier
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }

        /// <summary>
        /// The current price of this stock
        /// </summary>
        public Currency CurrentPrice
        {
            get { return currentPrice; }
        }

        /// <summary>
        /// A set of transactions made by the user relating to this stock.
        /// </summary>
        public List<StockTransaction> Transactions
        {
            get { return transactions; }
        }

        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }

        /// <summary>
        /// The number of shares currently owned by the user
        /// </summary>
        public double CurrentlyOwned
        {
            get
            {
                return transactions.Select(_.NumShares).Sum;
            }
        }

        /// <summary>
        /// The profit made by this user
        /// </summary>
        public double Profit
        {
            get 
            {
                return transactions.Select(_.TotalTransactionPrice).Sum;
            }
        }
    }
}
