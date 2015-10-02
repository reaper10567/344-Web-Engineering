using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace SE344.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
