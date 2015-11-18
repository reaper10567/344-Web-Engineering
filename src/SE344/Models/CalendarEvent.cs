﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE344.Models
{
    /// <summary>
    /// Class that encapsulates what a Calendar Event should consist of within the context of our project
    /// Is simply a data-holding class with no functionality as of right now.
    /// 
    /// -Tyler Gerber
    /// 10/27/2015
    /// </summary>
    public class CalendarEvent
    {
        //Our decided upon attributes
        private string nameOfEvent;
        private string startTime;
        private string endTime;
        private string description;
        private bool allDayEvent;

        /// <summary>
        /// Constructor for a CalendarEvent
        /// </summary>
        /// <param name="name">name of the event</param>
        /// <param name="start">start dateTime of the event</param>
        /// <param name="end">end dateTime of the event</param>
        /// <param name="desc">description of the event</param>
        public CalendarEvent(string name, string start, string end, string desc)
        {
            nameOfEvent = name;
            startTime = start;
            endTime = end;
            description = desc;
            allDayEvent = false;
        }
        /// <summary>
        /// Secondary Constructor for an all-day event
        /// </summary>
        /// <param name="name">name of the event</param>
        /// <param name="date">the day the event takes place</param>
        /// <param name="desc">description of the event</param>
        public CalendarEvent(string name, string date, string desc)
        {
            nameOfEvent = name;
            startTime = date;
            endTime = startTime;
            description = desc;
            allDayEvent = true;
        }

        //So many properties because private variables
        public string NameOfEvent
        {
            get { return nameOfEvent; }
            set { nameOfEvent = value; }
        }

        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool AllDayEvent
        {
            get { return allDayEvent; }
            set { allDayEvent = value; }
        }
    }
}
