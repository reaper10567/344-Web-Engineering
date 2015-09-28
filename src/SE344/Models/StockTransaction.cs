using System;

namespace SE344.Models
{
    /// <summary>
    /// A single purcase or sale of a particular stock.
    /// The stock that this applies to is derived from context - this is contained in a Stock, and thus needs no reference back to that stock.
    /// </summary>
    public class StockTransaction
    {
        public StockTransaction(DateTime transactionDate, decimal pricePerShare, int numShares)
        {
            TransactionDate = transactionDate;
            PricePerShare = pricePerShare;
            NumShares = numShares;
        }
        
        /// <summary>
        /// The date and time that transaction took place
        /// </summary>
        public DateTime TransactionDate { get; }
        
        /// <summary>
        /// The stock's price at the datetime that the transaction took place
        /// </summary>
        public decimal PricePerShare { get; }
        
        /// <summary>
        /// The number of shares bought at the time of the transaction.
        /// Will be a negative number if the transaction was a sale instead of a purchase
        /// </summary>
        public int NumShares { get; }
        
        /// <summary>
        /// The cost of the transaction
        /// </summary>
        public decimal TotalPrice
        {
            get { return (this.PricePerShare * this.NumShares); }
        }
    }
}
