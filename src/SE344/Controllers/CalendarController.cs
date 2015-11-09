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
        public ViewResult Thing(FormCollection form)
        {
            System.Diagnostics.Debug.WriteLine("Posting things here?");
            System.Diagnostics.Debug.WriteLine("Event Name: " + form["Event Name"]);
            return View("~/Views/Calendar/Index.cshtml");
        }
    }
}
