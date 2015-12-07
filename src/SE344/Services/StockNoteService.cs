using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SE344.Models;
using System.Data.SqlClient;

namespace SE344.Services
{
    public interface IStockNoteService
    {
        /// <summary>
        /// Retrieves the user's note about a stock, and places that note in the stock object
        /// </summary>
        /// <returns>the 'stock' parameter</returns>
        Task<Stock> getNote(ApplicationDbContext db, ApplicationUser user, Stock stock);

        /// <summary>
        /// stores the note from the stock object
        /// </summary>
        Task setNote(ApplicationDbContext db, ApplicationUser user, Stock stock);
    }

    public class StubStockNoteService : IStockNoteService {

        public async Task<Stock> getNote(ApplicationDbContext db, ApplicationUser user, Stock stock) {
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
        public async Task setNote(ApplicationDbContext db, ApplicationUser user, Stock stock) {
            System.Diagnostics.Debug.WriteLine(stock.Identifier + ": " + stock.Note);
        }
    }


    public class DbStockNoteService : IStockNoteService {
        public async Task<Stock> getNote(ApplicationDbContext db, ApplicationUser user, Stock stock) {
            string result = db.StockNotes.Where(x => (x.UserId.Equals(user.Id)) && (x.StockTicker == stock.Identifier)).Select(x => x.Note).FirstOrDefault();
            
            stock.Note = result;
            return stock;
        }

        public async Task setNote(ApplicationDbContext db, ApplicationUser user, Stock stock)
        {
            var note = db.StockNotes.Where(x => (x.UserId.Equals(user.Id)) && (x.StockTicker.Equals(stock.Identifier)));

            if (note.Count() == 0)
            {
                var newNote = new StockNote();
                newNote.Note = stock.Note;
                newNote.StockTicker = stock.Identifier;
                newNote.UserId = user.Id;

                db.StockNotes.Add(newNote);
            }
            else
            {
                var a = note.First();
                db.StockNotes.Update(a);
                a.Note = stock.Note;
            }
            db.SaveChanges();
        }
    }
}