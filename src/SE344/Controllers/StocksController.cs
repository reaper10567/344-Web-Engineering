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
using Newtonsoft.Json.Linq;
using SE344.Models;
using SE344.Services;
using SE344.ViewModels.Account;
using SE344.ViewModels.Stock;

namespace SE344.Controllers
{
    [Authorize]
    public class StocksController : Controller
    {
        readonly IStockHistoryService stockHistory = new StubStockHistoryService();
        readonly IStockInformationService stockInfo = new YahooStockInformationService();


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allIds = stockHistory.getKnownIdentifiers();
            var allStocks = await Task.WhenAll(allIds.Select(stockInfo.GetQuoteAsync));

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
        [HttpGet]
        public async Task<IActionResult> SearchStocks(string symbol)
        {
            var model = new SearchViewModel
            {
                Symbol = symbol
            };

            if (symbol == null)
            {
                return View(model);
            }

            ViewData["symbol"] = symbol;
            try
            {
                // get a week of data
                var history = JArray.Parse(await stockInfo.GetHistoryInfoAsync(symbol, DateTime.Now.AddMonths(-1), DateTime.Now));
                ViewData["ChartData"] = history;
                ViewData["LowHighData"] = history.Select(t =>
                {
                    var quote = (JObject) t;
                    return new JArray
                    {
                        decimal.Round((decimal) quote["Open"], 2),
                        decimal.Round((decimal) quote["High"], 2),
                        decimal.Round((decimal) quote["Low"], 2),
                        decimal.Round((decimal) quote["Close"], 2)
                    };
                });
            }
            catch (UnknownTickerSymbolException e)
            {
                ViewData["ChartData"] = new JArray();
                ViewData["LowHighData"] = new List<JArray>();
            }
            model = await stockInfo.GetQuoteAsync(symbol);

            return View(model);
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
