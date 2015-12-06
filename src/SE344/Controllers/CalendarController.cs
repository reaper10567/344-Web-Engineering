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
        static CalendarEvent eve = null;
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Number Of Events: " + eventsForUser.Count);
            //do call to DB here
            var model = new List<EventViewModel>();
            foreach (var e in eventsForUser) {
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


        public IActionResult EventToJSON(FormCollection form)
        {
            eve = null;
            string name = form["nameOfEvent"];
            foreach (CalendarEvent e in eventsForUser)
            {

                if (e.NameOfEvent == name)
                {
                    eve = e;
                    break;
                }

            }
            System.Diagnostics.Debug.WriteLine(Request.Path);
            System.Diagnostics.Debug.WriteLine(Request.PathBase);
            return Redirect("~/Calendar#modalDialog3");
        }

        [HttpGet]
        public IActionResult ToJSON() {
            if (eve != null)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(eve);
                System.Diagnostics.Debug.WriteLine(json);
                return File(json, "text/json", eve.NameOfEvent + ".json");
            }
            else
            {
                return Redirect("~/Calendar#modalDialog3");
            }
        }
       

        [HttpPost]
        public IActionResult Thing(FormCollection form)
        {

            bool allDay;
            Boolean.TryParse(form["allDay"], out allDay);
            string name = form["Event Name"];
            string start = form["StartDateTime"];
            string end = form["EndDateTime"];

            if (allDay) {
                eventsForUser.Add(new CalendarEvent(name,start));

            }
            else
            {
                eventsForUser.Add(new CalendarEvent(name,start,end));
            }
            return RedirectToAction("Index");
        }

        // trying this out to edit events which are already on the calendar----------------------------------------
        [HttpPost]
        public IActionResult SecondThing(FormCollection form)
        {
            bool o_allDay;
            Boolean.TryParse(form["original_allDay"], out o_allDay);
            string o_name = form["original_title"];
            string o_start = form["original_start"];
            string o_end = form["original_end"];

            bool allDay;
            Boolean.TryParse(form["allDay1"], out allDay);
            string name = form["Event Name"];
            string start = form["StartDateTime"];
            string end = form["EndDateTime"];

            foreach (CalendarEvent e in eventsForUser)
            {
                
                if (e.NameOfEvent.Equals(o_name))
                    { 
                    //e.NameOfEvent.Equals(o_name) && e.StartTime.Equals(o_start) && e.EndTime.Equals(o_end) && e.AllDayEvent.Equals(o_allDay))
                
                    eventsForUser.Remove(e);
                    if (allDay)
                    {
                        eventsForUser.Add(new CalendarEvent(name, start));

                    }
                    else
                    {
                        eventsForUser.Add(new CalendarEvent(name, start, end));
                    }
                    return RedirectToAction("Index");
                    
                }

            }
            
            return RedirectToAction("Index");
        }
        // end -----------------------------------------------------------------------------------------------------

    }
}
