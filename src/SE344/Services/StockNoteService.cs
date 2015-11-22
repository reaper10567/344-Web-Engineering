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
        Task<Stock> getNote(string userId, Stock stock);

        /// <summary>
        /// stores the note from the stock object
        /// </summary>
        Task setNote(string userId, Stock stock);
    }

    public class StubStockNoteService : IStockNoteService {

        public async Task<Stock> getNote(string userId, Stock stock) {
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
        public async Task setNote(string userId, Stock stock) {
            System.Diagnostics.Debug.WriteLine(stock.Identifier + ": " + stock.Note);
        }
    }

/*
    public class DbStockNoteService : IStockNoteService {
        public async Task<Stock> getNote(string userId, Stock stock) {
            SqlCommand query = new SqlCommand(
                "SELECT TOP 1 [note_content] FROM [hodge_podge].[dbo].[stock_notes] WHERE [User_ID] = @UserId AND [ticker_symbol] = @Symbol"
            );
            query.Parameters.AddWithValue("@UserId", userId);
            query.Parameters.AddWithValue("@Symbol", stock.Identifier);

            var result = query.ExecuteScalarAsync();
            stock.Note = (await result).ToString();
            return stock;
        }
        public async Task setNote(string userId, Stock stock)
        {
            SqlCommand rowToUpdateQuery = new SqlCommand(
                "SELECT TOP 1 [note_ID] FROM [hodge_podge].[dbo].[stock_notes] WHERE [User_ID] = @UserId AND [ticker_symbol] = @Symbol"
            );
            rowToUpdateQuery.Parameters.AddWithValue("@UserId", userId);
            rowToUpdateQuery.Parameters.AddWithValue("@Symbol", stock.Identifier);

            var rowToUpdate = await rowToUpdateQuery.ExecuteScalarAsync();

            SqlCommand updateQuery;
            if (rowToUpdate == null)
            {
                // then no row with specified userid/sumbol combination exists
                // therefore, create a new row with the data to create
                updateQuery = new SqlCommand(
                    "INSERT INTO [hodge_podge].[dbo].[stock_notes] ([note_ID], [User_ID], [ticker_symbol], [note_content]) VALUES (DEFAULT, @UserId, @Symbol, @Note)"
                );
                updateQuery.Parameters.AddWithValue("@UserId", userId);
                updateQuery.Parameters.AddWithValue("@Symbol", stock.Identifier);
                updateQuery.Parameters.AddWithValue("@Note", stock.Note);
            }
            else
            {
                // then row with specified userid/sumbol combination exists
                // therefore, update that row for new note
                updateQuery = new SqlCommand(
                    "UPDATE [hodge_podge].[dbo].[stock_notes] SET [note_content] = @Note WHERE [note_ID] = @Id"
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