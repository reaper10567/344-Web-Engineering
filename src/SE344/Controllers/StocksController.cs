using Microsoft.AspNet.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using SE344.Models;
using SE344.Services;
using SE344.ViewModels.Account;

namespace SE344.Controllers
{
    [Authorize]
    public class StocksController : Controller
    {
        readonly IStockHistoryService stockHistory = new StubStockHistoryService();
        readonly IStockInformationService stockInfo = new YahooStockInformationService();


        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            var allIds = stockHistory.getKnownIdentifiers();
            var allStocks = allIds.Select(CreateStock);
            allStocks.OrderBy(async (a) => (await a).CurrentlyOwned);

            ViewData["stocks"] = allStocks;
            return View();
        }

        #region "transaction history"
/*
        // POST: /Stock/BuyStock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(Stock s, int shares)
        {
            EnsureDatabaseCreated(_applicationDbContext);
            return View();
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult History()
        {
            return View();
        }

        //???: this is a different page, right?
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult HistoryCvs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearHistory()
        {
            EnsureDatabaseCreated(_applicationDbContext);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoadHistory()
        {
            EnsureDatabaseCreated(_applicationDbContext);
            return View();
        }
*/
        #endregion

        // Theoretically, this should be GET, but I'm not sure whether ASP.NET supports get forms
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchStocks([FromBody] string symbol)
        {
			ViewData["symbol"] = symbol;
            ViewData["result"] = await CreateStock(symbol);
            return View();
        }



        private async Task<Stock> CreateStock(string id)
        {
            Stock retVal = new Stock(id, await stockInfo.CurrentPrice(id));
            retVal.Transactions.AddRange(stockHistory.getTransactions(id));
            // TODO: retVal.note = ???

            return retVal;
        }

        private int CompareStockByCurrentHoldings(Stock a, Stock b)
        {
            return (a.CurrentPrice * a.CurrentlyOwned).CompareTo(b.CurrentPrice * b.CurrentlyOwned);
        }
    }
}
