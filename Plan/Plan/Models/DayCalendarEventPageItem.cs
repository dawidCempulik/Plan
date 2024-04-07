using System;
using System.Collections.Generic;
using System.Text;

namespace Plan.Models
{
    public class DayCalendarEventPageItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string TimeStartLabel { get; set; }
        public string TimeEndLabel { get; set; }

        public DayCalendarEventPageItem(int id, string text, string description, string timeStartLabel, string timeEndLabel) { 
            Id = id;
            Text = text;
            Description = description;
            TimeStartLabel = timeStartLabel;
            TimeEndLabel = timeEndLabel;
        }
    }
}
