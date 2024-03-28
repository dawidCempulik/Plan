﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Plan.Models
{
    public class CalendarEventPageItem
    {
        public string Text { get; set; }
        public string TimeLabel { get; set; }

        public CalendarEventPageItem(string text, string timeLabel) { 
            Text = text;
            TimeLabel = timeLabel;
        }
    }
}