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

        public long Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string StockTicker { get; set; }
        
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

        public override int GetHashCode()
        {
            return ((TransactionDate.GetHashCode() * 31) + PricePerShare.GetHashCode() + 31) * NumShares;
        }

        protected bool CanEquals(object rhs)
        {
            return rhs.GetType() == typeof(StockTransaction);
        }

        public override bool Equals(object rhs)
        {
            if (this.CanEquals(rhs))
            {
                var rhs2 = (StockTransaction)rhs;
                if (rhs2.CanEquals(this))
                {
                    return (this.TransactionDate.Equals(rhs2.TransactionDate)) &&
                           (this.PricePerShare.Equals(rhs2.PricePerShare)) &&
                           (this.NumShares.Equals(rhs2.NumShares));
                }
            }
            return false;
        }
    }
}
