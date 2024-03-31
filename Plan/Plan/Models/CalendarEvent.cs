using System;
using System.Collections.Generic;
using System.Text;

namespace Plan.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string Repeat { get; set; }

        public CalendarEvent() { }

        public CalendarEvent(int id, string text, string description, DateTime dateTimeStart, DateTime dateTimeEnd, string repeat)
        {
            Id = id;
            Text = text;
            Description = description;
            DateTimeStart = dateTimeStart;
            DateTimeEnd = dateTimeEnd;
            Repeat = repeat;
        }
    }
}
