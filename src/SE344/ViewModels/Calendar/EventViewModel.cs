using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE344.ViewModels.Calendar
{
    public class EventViewModel
    {
        public string Title { get; set; }
        public bool AllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
