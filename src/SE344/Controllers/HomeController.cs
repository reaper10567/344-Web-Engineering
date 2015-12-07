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
using SE344.Models;
using Microsoft.AspNet.Identity;

namespace SE344.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IStockHistoryService _stockHistory;
        private readonly IStockInformationService _stockInfo;
        private readonly FacebookApiService _facebookApi = new FacebookApiService();

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IStockHistoryService stockHistory,
            IStockInformationService stockInfo,
            ApplicationDbContext applicationDbContext)
        {
            _stockHistory = stockHistory;
            _stockInfo = stockInfo;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            _facebookApi.AccessToken = Context.User.FindFirstValue("access_token");

            var allIds = _stockHistory.getKnownIdentifiers(_applicationDbContext, await GetCurrentUserAsync());
            var allStocks = Task.WhenAll(allIds.Select(x => new Stock(x)).Select(_stockInfo.GetQuoteAsync));

            var facebookFeedTask = _facebookApi.GetUserFeedAsync();

            ViewData["Stocks"] = await allStocks;
            ViewData["Feed"] = await facebookFeedTask;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string message)
        {
            _facebookApi.AccessToken = Context.User.FindFirstValue("access_token");
            await _facebookApi.PostUserFeedAsync(message);
            return RedirectToAction("Index");
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

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(Context.User.GetUserId());
        }
    }
}
