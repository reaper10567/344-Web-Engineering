using System;

namespace SE344.Models
{
    /// <summary>
    /// A single purcase or sale of a particular stock.
    /// The stock that this applies to is derived from context - this is contained in a Stock, and thus needs no reference back to that stock.
    /// </summary>
    public class StockTransaction
    {
        public StockTransaction(DateTime transactionDate, decimal priceAtTransactionTime, int numShares)
        {
            this.transactionDate = transactionDate;
            this.priceAtTransactionTime = priceAtTransactionTime;
            this.numShares = numShares;
        }

        private readonly DateTime transactionDate;
        private readonly decimal priceAtTransactionTime;
        private readonly int numShares;
        
        /// <summary>
        /// The date and time that transaction took place
        /// </summary>
        public DateTime TransactionDate
        {
            get { return this.transactionDate; }
        }
        
        /// <summary>
        /// The stock's price at the datetime that the transaction took place
        /// </summary>
        public decimal PriceAtTransactionTime
        {
            get { return this.priceAtTransactionTime; }
        }
        
        /// <summary>
        /// The number of shares bought at the time of the transaction.
        /// Will be a negative number if the transaction was a sale instead of a purchase
        /// </summary>
        public int NumShares
        {
            get { return this.numShares; }
        }
        
        /// <summary>
        /// The cost of the transaction
        /// </summary>
        public decimal TotalTransactionPrice
        {
            get { return (this.PriceAtTransactionTime * this.NumShares); }
        }
    }
}
