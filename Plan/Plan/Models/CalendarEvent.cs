using System;
using System.Collections.Generic;
using System.Text;

namespace Plan.Models
{
    public class CalendarEvent
    {
        public CalendarEvent() { }

        public CalendarEvent(int id, string text, string description, DateTime dateTime, string repeat)
        {
            Id = id;
            Text = text;
            Description = description;
            DateTime = dateTime;
            Repeat = repeat;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Repeat { get; set; }
    }
}
