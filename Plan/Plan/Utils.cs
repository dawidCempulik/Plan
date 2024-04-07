using System;
using System.Collections.Generic;
using System.Text;

namespace Plan
{
    public static class Utils
    {
        public static bool DateRangeOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 >= start2 && start1 <= end2) // start of the event inside the range
            {
                return true;
            }
            if (end1 >= start2 && end1 <= end2) // end of the event inside the range
            {
                return true;
            }
            if (start1 <= start2 && end1 >= end2) // event surrounds the range
            {
                return true;
            }

            return false;
        }
    }
}
