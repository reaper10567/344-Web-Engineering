using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace SE344.Controllers
{
    [Authorize]
    public class StocksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
