using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SE344.Models;

namespace SE344.Services
{
    public interface IStockNoteService
    {
        /// <summary>
        /// Retrieves the user's note about a stock, and places that note in the stock object
        /// </summary>
        /// <returns>the 'stock' parameter</returns>
        Task<Stock> getNote(Stock stock);

        /// <summary>
        /// stores the note from the stock object
        /// </summary>
        Task setNote(Stock stock);
    }

    public class StubStockNoteService : IStockNoteService {

        public async Task<Stock> getNote(Stock stock) {
            if (stock.Identifier == "F")
            {
                stock.Note = "Ford Motor Comp";
            }
            if (stock.Identifier == "TWTR")
            {
                stock.Note = "Who needs 140 characters?";
            }

            return stock;
        }
        public async Task setNote(Stock stock) {
            Console.Out.WriteLine(stock.Identifier + ": " + stock.Note);
        }
    }

/*
    public class DbStockNoteService : IStockNoteService {
        public async Task<Stock> getNote(Stock stock) {
            
        }
        public async Task<Unit> setNote(Stock stock) {
            
        }
    }
*/
}