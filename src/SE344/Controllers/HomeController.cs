using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using SE344.Services;
using SE344.Services.Facebook;

namespace SE344.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly IStockHistoryService _stockHistory = new StubStockHistoryService();
        readonly IStockInformationService _stockInfo = new YahooStockInformationService();
        private readonly FacebookApiService _facebookApi = new FacebookApiService();

        public async Task<IActionResult> Index()
        {
            _facebookApi.AccessToken = Context.User.FindFirstValue("access_token");

            var allIds = _stockHistory.getKnownIdentifiers();
            var allStocksTasks = allIds.Select(_stockInfo.GetQuoteAsync);
            var facebookFeedTask = _facebookApi.GetUserFeedAsync();

            ViewData["Stocks"] = await Task.WhenAll(allStocksTasks);
            ViewData["Feed"] = await facebookFeedTask;
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
