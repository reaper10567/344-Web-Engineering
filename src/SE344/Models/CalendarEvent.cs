using System;

namespace SE344.Models
{
    /// <summary>
    /// <para>
    /// Class that encapsulates what a Calendar Event should consist of within the context of our project
    /// Is simply a data-holding class with no functionality as of right now.
    /// </para>
    /// -Tyler Gerber
    /// 10/27/2015
    /// </summary>
    public class CalendarEvent
    {
        public CalendarEvent()
        {
        }

        /// <summary>
        /// Constructor for a CalendarEvent
        /// </summary>
        /// <param name="name">name of the event</param>
        /// <param name="start">start dateTime of the event</param>
        /// <param name="end">end dateTime of the event</param>
        public CalendarEvent(string name, string start, string end)
        {
            NameOfEvent = name;
            StartTime = DateTime.Parse(start);
            EndTime = DateTime.Parse(end);
            AllDayEvent = false;
        }

        /// <summary>
        /// Secondary Constructor for an all-day event
        /// </summary>
        /// <param name="name">name of the event</param>
        /// <param name="date">the day the event takes place</param>
        public CalendarEvent(string name, string date)
        {
            NameOfEvent = name;
            StartTime = DateTime.Parse(date);
            EndTime = StartTime;
            AllDayEvent = true;
        }

        public long Id { get; set; }

        public string NameOfEvent { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool AllDayEvent { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
