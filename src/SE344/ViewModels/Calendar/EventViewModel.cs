using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE344.ViewModels.Calendar
{
    public class EventViewModel
    {
        public string title { get; set; }
        public string allDay { get; set; }
        public string start { get; set; }
        public string end { get; set; }

    }
}
