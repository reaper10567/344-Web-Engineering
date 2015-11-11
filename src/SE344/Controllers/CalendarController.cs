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
        List<CalendarEvent> eventsForUser = new List<CalendarEvent>();
        public ActionResult Index()
        {
            //do call to DB here
            var model = new EventViewModel[eventsForUser.Count+1];
            for(int i = 0; i < eventsForUser.Count; i++)
            {
                model[i] = new EventViewModel
                {
                    Title = eventsForUser[i].NameOfEvent,
                    AllDay = eventsForUser[i].AllDayEvent,
                    Start = eventsForUser[i].StartTime,
                    End = eventsForUser[i].EndTime
                    
                };
            }
            ViewData["Events"] = eventsForUser;
            //use viewmodel instead of c# object
            return View(model);
        }

       

        [HttpPost]
        public ViewResult Thing(FormCollection form)
        {
            System.Diagnostics.Debug.WriteLine("Posting things here?");
            System.Diagnostics.Debug.WriteLine("Event Name: " + form["Event Name"]);

            bool allDay;
            Boolean.TryParse(form["allDay"], out allDay);
            string name = form["Event Name"];
            DateTime start;
            DateTime.TryParse(form["StartDateTime"], out start);
            DateTime end;
            DateTime.TryParse(form["EndDateTime"], out end);
            string desc = form["EventDescription"];

            if (allDay) {
                eventsForUser.Add(new CalendarEvent(name,start,desc));
            }
            else
            {
                eventsForUser.Add(new CalendarEvent(name,start,end,desc));
            }
            System.Diagnostics.Debug.WriteLine("Number Of Events: " + eventsForUser.Count);
            return View("~/Views/Calendar/Index.cshtml");
        }
    }
}
