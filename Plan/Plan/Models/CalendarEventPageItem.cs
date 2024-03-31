using System;
using System.Collections.Generic;
using System.Text;

namespace Plan.Models
{
    public class CalendarEventPageItem
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public string TimeStartLabel { get; set; }
        public string TimeEndLabel { get; set; }

        public CalendarEventPageItem(string text, string description, string timeStartLabel, string timeEndLabel) { 
            Text = text;
            Description = description;
            TimeStartLabel = timeStartLabel;
            TimeEndLabel = timeEndLabel;
        }
    }
}
