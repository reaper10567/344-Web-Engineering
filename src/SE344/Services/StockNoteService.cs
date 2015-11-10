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
            System.Diagnostics.Debug.WriteLine(stock.Identifier + ": " + stock.Note);
        }
    }

/*
    public class DbStockNoteService : IStockNoteService {
        public async Task<Stock> getNote(Stock stock) {
            SqlCommand query = new SqlCommand(
                "SELECT content FROM StockNotes WHERE UserID = @UserId AND TickerSymbol = @Symbol"
            );
            query.Parameters.AddWithValue("@UserId", ???);
            query.Parameters.AddWithValue("@Symbol", stock.Identifier);

            var result = query.ExecuteScalarAsync();
            stock.Note = (await result).ToString();
            return stock;
        }
        public async Task setNote(Stock stock)
        {
            SqlCommand rowToUpdateQuery = new SqlCommand(
                "SELECT id FROM StockNotes WHERE UserID = @UserId AND TickerSymbol = @Symbol"
            );
            rowToUpdateQuery.Parameters.AddWithValue("@UserId", ???);
            rowToUpdateQuery.Parameters.AddWithValue("@Symbol", stock.Identifier);

            var rowToUpdate = await rowToUpdateQuery.ExecuteScalarAsync();

            SqlCommand updateQuery;
            if (rowToUpdate == null)
            {
                // then no row with specified userid/sumbol combination exists
                // therefore, create a new row with the data to create
                updateQuery = new SqlCommand(
                   "INSERT INTO StockNotes WITH UserID = @UserId AND TickerSymbol = @Symbol AND Content = @Note"
                );
                updateQuery.Parameters.AddWithValue("@UserId", ???);
                updateQuery.Parameters.AddWithValue("@Symbol", stock.Identifier);
                updateQuery.Parameters.AddWithValue("@Note", stock.Note);
            }
            else
            {
                // then row with specified userid/sumbol combination exists
                // therefore, update that row for new note
                updateQuery = new SqlCommand(
                    "UPDATE StockNotes SET Contents = @Note WHERE id = @Id"
                );
                updateQuery.Parameters.AddWithValue("@Id", rowToUpdate);
                updateQuery.Parameters.AddWithValue("@Note", stock.Note);
            }

            var updateQueryResult = updateQuery.ExecuteNonQueryAsync();
            await updateQueryResult;
        }
    }
*/
}