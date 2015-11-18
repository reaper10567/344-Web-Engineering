using System;
using SE344.Controllers;
using Xunit;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using SE344.Services;
using SE344.Models;

namespace SE344Test.Controllers
{
    public class StocksControllerTest
    {

        [Fact]
        public async Task IndexDoesNotError()
        {
            var dut = new StocksController(
                null,
                new StubStockHistoryService(),
                new YahooStockInformationService(),
                new StubStockNoteService(),
                null
            );
            var result = await dut.Index() as ViewResult;
            Assert.True(result.ViewData.ContainsKey("stocks"));
            // the specific value is not deterministic, but the value hould exist
        }

        [Theory]
        [InlineData("YHOO")]
        [InlineData("F")]
        [InlineData("1234")]
        [InlineData("\"")]
        [InlineData(";")]
        [InlineData("XXXX")]
        public async Task SearchStocksDoesNotError(string s)
        {
            var dut = new StocksController(
                null,
                new StubStockHistoryService(),
                new YahooStockInformationService(),
                new StubStockNoteService(),
                null
            );
            var result = await dut.SearchStocks(s) as ViewResult;
            Assert.Equal(s, result.ViewData["symbol"]);
            Assert.True(result.ViewData.ContainsKey("ChartData"));
            Assert.True(result.ViewData.ContainsKey("LowHighData"));
        }

        [Fact]
        public void HistoryDoesNotError()
        {
            var stockHistory = new StubStockHistoryService();
            var dut = new StocksController(
                null,
                stockHistory,
                null,
                null,
                null
            );
            var result = dut.History() as ViewResult;
            Assert.Equal(stockHistory.getTransactions(), result.ViewData["transactions"]);
        }

        [Fact]
        public void HistoryCsvDoesNotError()
        {
            var stockHistory = new StubStockHistoryService();
            var dut = new StocksController(
                null,
                stockHistory,
                new YahooStockInformationService(),
                new StubStockNoteService(),
                null
            );
            var result = dut.HistoryCvs() as FileStreamResult;
            Assert.Equal(result.FileDownloadName, "transactionHistory.csv");
        }

        [Fact]
        public void SetNote()
        {
            var stockNote = new MockStockNote();
            var dut = new StocksController(
                null,
                null,
                null,
                stockNote,
                null
            );
            var result = dut.SetNote("WHEE", "sounds legit");
            Assert.Equal("WHEE", stockNote.Symbol);
            Assert.Equal("sounds legit", stockNote.Note);
            Assert.True(stockNote.wasCalled);
        }
    }

    class MockStockNote : IStockNoteService
    {
        public bool wasCalled { get; set; } = false;
        public String Note { get; set; }
        public String Symbol { get; set; }

        public Task<Stock> getNote(Stock stock)
        {
            stock.Note = this.Note;
            return new Task<Stock>(() => stock);
        }
        
        public Task setNote(Stock stock)
        {
            if (wasCalled) { throw new Exception("SetNote called twice"); }
            this.wasCalled = true;
            this.Note = stock.Note;
            this.Symbol = stock.Identifier;
            return new Task(() => { });
        }
    }
}
