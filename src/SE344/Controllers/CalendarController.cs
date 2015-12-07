using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using Microsoft.AspNet.Http.Internal;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using SE344.Models;
using SE344.ViewModels.Calendar;
namespace SE344.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CalendarController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //System.Diagnostics.Debug.WriteLine("Number Of Events: " + eventsForUser.Count);
            var model = new List<EventViewModel>();
            var user = await GetCurrentUserAsync();

            foreach (var e in _db.Events.Include(e => e.User).Where(e => e.UserId.Equals(user.Id)))
            {
                System.Diagnostics.Debug.WriteLine(e);
                var evm = new EventViewModel
                {
                    title = e.NameOfEvent,
                    allDay = e.AllDayEvent.ToString().ToLower(),
                    start = e.StartTime.ToString("O"),
                    end = e.EndTime.ToString("O")
                };
                model.Add(evm);
            }

            var modelToReturn = model.ToArray();
            ViewData["Events"] = modelToReturn;
            //use viewmodel instead of c# object
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddEvent(FormCollection form)
        {

            bool allDay;
            Boolean.TryParse(form["allDay"], out allDay);
            string name = form["Event Name"];
            string start = form["StartDateTime"];
            string end = form["EndDateTime"];

            var user = await GetCurrentUserAsync();

            if (allDay)
            {
                _db.Events.Add(new CalendarEvent(name, start)
                {
                    UserId = user.Id
                });

            }
            else
            {
                _db.Events.Add(new CalendarEvent(name, start, end)
                {
                    UserId = user.Id
                });
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // trying this out to edit events which are already on the calendar----------------------------------------
        [HttpPost]
        public async Task<IActionResult> ChangeEvent(FormCollection form)
        {
            bool oAllDay;
            Boolean.TryParse(form["original_allDay"], out oAllDay);
            string oName = form["original_title"];
            var oStart = DateTime.Parse(form["original_start"]);
            var oEnd = DateTime.Parse(form["original_end"]);

            bool allDay;
            Boolean.TryParse(form["allDay1"], out allDay);
            string name = form["Event Name"];
            var start = DateTime.Parse(form["StartDateTime"]);
            DateTime end;
            try
            {
                end = DateTime.Parse(form["EndDateTime"]);
            }
            catch (Exception)
            {

                end = start;
            }

            var user = await GetCurrentUserAsync();
            var events = _db.Events.Include(e => e.User)
                .Where(e => e.UserId.Equals(user.Id))
                .Where(e => e.StartTime.Equals(oStart) && e.EndTime.Equals(oEnd));

            foreach (var e in events)
            {
                e.AllDayEvent = allDay;
                e.NameOfEvent = name;
                e.StartTime = allDay ? start.Date : start;
                e.EndTime = allDay ? e.StartTime.AddDays(1) : end;
                _db.Events.Update(e);
                //return RedirectToAction("Index");
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // end -----------------------------------------------------------------------------------------------------

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(Context.User.GetUserId());
        }
    }
}
