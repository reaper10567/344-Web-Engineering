using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using SE344.Services;

namespace SE344.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly IStockHistoryService _stockHistory = new StubStockHistoryService();
        readonly IStockInformationService _stockInfo = new YahooStockInformationService();

        public async Task<IActionResult> Index()
        {
            var allIds = _stockHistory.getKnownIdentifiers();
            var allStocks = await Task.WhenAll(allIds.Select(_stockInfo.GetQuoteAsync));

            ViewData["Stocks"] = allStocks;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
