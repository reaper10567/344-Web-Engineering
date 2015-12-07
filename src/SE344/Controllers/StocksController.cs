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

namespace SE344.Controllers
{
    [Authorize]
    public class StocksController : Controller
    {
        private readonly IStockHistoryService stockHistory;
        private readonly IStockInformationService stockInfo;
        private readonly IStockNoteService stockNote;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public StocksController(
            UserManager<ApplicationUser> userManager,
            IStockHistoryService stockHistory,
            IStockInformationService stockInfo,
            IStockNoteService stockNote,
            ApplicationDbContext applicationDbContext)
        {
            this.stockHistory = stockHistory;
            this.stockInfo = stockInfo;
            this.stockNote = stockNote;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allIds = stockHistory.getKnownIdentifiers(_applicationDbContext, await GetCurrentUserAsync());
            var allStocks = await Task.WhenAll(allIds.Select(x => new Stock(x)).Select(stockInfo.GetQuoteAsync));

            ViewData["stocks"] = allStocks;
            return View();
        }

        #region "transaction history"

        // POST: /Stock/BuyStock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(string symbol, int shares)
        {
            var stock = new Stock(symbol);
            stock = await stockInfo.GetQuoteAsync(stock);

            var model = new StockTransaction(DateTime.Now, stock.CurrentPrice.Value, shares);
            stockHistory.addTransaction(_applicationDbContext, await GetCurrentUserAsync(), stock, model);

            return Redirect("/Stocks/SearchStocks?symbol=" + symbol);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            ViewData["transactions"] = stockHistory.getTransactions(_applicationDbContext, await GetCurrentUserAsync());
            return View();
        }

        // http://stackoverflow.com/questions/6775248/export-to-csv-from-mvc-controller-and-view-displays-csv-raw-data-on-page
        [HttpGet]
        public async Task<IActionResult> HistoryCvs()
        {
            var transactions = stockHistory.getTransactions(_applicationDbContext, await GetCurrentUserAsync());

            // What? MVC? Why would ASP.NET allow that?
            var retVal = new System.IO.MemoryStream();
            {
                var writer = new System.IO.StreamWriter(retVal);
                writer.WriteLine("\"Ticker Symbol\",\"Datetime\",\"Price Per Share\",\"Num Shares\"");
                foreach (var line in transactions.ToList())
                {
                    writer.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                                line.StockTicker, line.TransactionDate,
                                line.PricePerShare, line.NumShares
                    ));
                }
                writer.Flush();
            }
            retVal.Seek(0, System.IO.SeekOrigin.Begin);

            return File(retVal, "text/csv", "transactionHistory.csv");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearHistory()
        {
            stockHistory.clear(_applicationDbContext, await GetCurrentUserAsync());
            return Redirect("/Stocks/History");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadHistory(System.Web.HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var reader = new Microsoft.VisualBasic.FileIO.TextFieldParser(file.InputStream);
                reader.SetDelimiters(",");
                while (!reader.EndOfData)
                {
                    var line = reader.ReadFields();

                    var stock = new Stock(line[0]);
                    var model = new StockTransaction(DateTime.Parse(line[1]), Decimal.Parse(line[2]), int.Parse(line[3]));
                    stockHistory.addTransaction(_applicationDbContext, await GetCurrentUserAsync(), stock, model);
                }
            }

            return Redirect("/Stocks/History");
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> SearchStocks(string symbol)
        {
            var model = new Stock(symbol);

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
            model = await stockInfo.GetQuoteAsync(model);
            model = await stockNote.getNote(_applicationDbContext, await GetCurrentUserAsync(), model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetNote(string symbol, string note)
        {
            var model = new Stock(symbol);
            model.Note = note;
            await stockNote.setNote(_applicationDbContext, await GetCurrentUserAsync(), model);

            return Redirect("/Stocks/SearchStocks?symbol=" + symbol);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(Context.User.GetUserId());
        }
    }

}
