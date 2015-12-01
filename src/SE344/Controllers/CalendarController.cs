using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using Microsoft.AspNet.Http.Internal;
using System.Collections.Generic;
using SE344.Models;
using SE344.ViewModels.Calendar;

namespace SE344.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        static List<CalendarEvent> eventsForUser = new List<CalendarEvent>();
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Number Of Events: " + eventsForUser.Count);
            //do call to DB here
            var model = new List<EventViewModel>();
            foreach(var e in eventsForUser){
                var evm = new EventViewModel
                {
                    title = e.NameOfEvent,
                    allDay = e.AllDayEvent.ToString().ToLower(),
                    start = e.StartTime,
                    end = e.EndTime
                };
                model.Add(evm);
            }

            var modelToReturn = model.ToArray();
            ViewData["Events"] = modelToReturn;
            //use viewmodel instead of c# object
            return View();
        }

       

        [HttpPost]
        public IActionResult Thing(FormCollection form)
        {

            bool allDay;
            Boolean.TryParse(form["allDay"], out allDay);
            string name = form["Event Name"];
            string start = form["StartDateTime"];
            string end = form["EndDateTime"];
            string desc = form["EventDescription"];

            if (allDay) {
                eventsForUser.Add(new CalendarEvent(name,start,desc));

            }
            else
            {
                eventsForUser.Add(new CalendarEvent(name,start,end,desc));
            }
            return RedirectToAction("Index");
        }
    }
}
