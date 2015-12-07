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
        IEnumerable<string> getKnownIdentifiers(ApplicationDbContext db, ApplicationUser user);

        /// <summary>
        /// Return all transactions
        /// </summary>
        IEnumerable<StockTransaction> getTransactions(ApplicationDbContext db, ApplicationUser user);

        /// <summary>
        /// Return transactions filtered by identifier
        /// </summary>
        IEnumerable<StockTransaction> getTransactions(ApplicationDbContext db, ApplicationUser user, string identifier);

        void addTransaction(ApplicationDbContext db, ApplicationUser user, Stock stock, StockTransaction model);

        void clear(ApplicationDbContext db, ApplicationUser user);
    }

    public class StubStockHistoryService
    {
        public IEnumerable<string> getKnownIdentifiers(ApplicationDbContext db, ApplicationUser user)
        {
            return this.getTransactions(db, user).ToList().Select(x => x.Key).Distinct();
        }

        public IEnumerable<StockTransaction> getTransactions(ApplicationDbContext db, ApplicationUser user, string identifier)
        {
            return this.getTransactions(db, user).ToList().FindAll(x => x.Key == identifier).Select(x => x.Value);
        }

        public IEnumerable<KeyValuePair<string, StockTransaction>> getTransactions(ApplicationDbContext db, ApplicationUser user)
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

        public void addTransaction(ApplicationDbContext db, ApplicationUser user, Stock stock, StockTransaction model) { }

        public void clear(ApplicationDbContext db, ApplicationUser user) { }
    }

    public class DbStockHistoryService : IStockHistoryService
    {
        public IEnumerable<StockTransaction> getTransactions(ApplicationDbContext db, ApplicationUser user)
        {
            return db.StockTransactions.Where(x => x.UserId.Equals(user.Id));
        }
        public IEnumerable<StockTransaction> getTransactions(ApplicationDbContext db, ApplicationUser user, string tickerSymbol)
        {
            return db.StockTransactions.Where(x => x.StockTicker.Equals(tickerSymbol) && x.UserId.Equals(user.Id));
        }
        public IEnumerable<string> getKnownIdentifiers(ApplicationDbContext db, ApplicationUser user)
        {
            var transactions = this.getTransactions(db, user);
            var ids = transactions.Select(x => x.StockTicker).Distinct().ToList();

            var idPrice = ids.Select(x => Tuple.Create(x, transactions.Where(y => y.StockTicker.Equals(x)).Select(y => y.TotalPrice).Sum()));

            return idPrice.OrderBy(x => x.Item2).Select(x => x.Item1).Take(6);
        }
        
        public void addTransaction(ApplicationDbContext db, ApplicationUser user, Stock stock, StockTransaction model) {
            model.StockTicker = stock.Identifier;
            model.UserId = user.Id;
            db.StockTransactions.Add(model);
            db.SaveChanges();
        }

        public void clear(ApplicationDbContext db, ApplicationUser user)
        {
            var elemsToRemove = db.StockTransactions.Where(x => x.UserId.Equals(user.Id));
            elemsToRemove.Select(x => db.StockTransactions.Remove(x));
            db.SaveChanges();
        }
    }
}
