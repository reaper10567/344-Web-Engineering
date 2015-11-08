using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using Microsoft.AspNet.Http.Internal;

namespace SE344.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Thing(FormCollection form)
        {
            System.Diagnostics.Debug.WriteLine("Mother fucking posting stuff man!");
            System.Diagnostics.Debug.WriteLine("Event Name: " + form["Event Name"]);
        }
    }
}
