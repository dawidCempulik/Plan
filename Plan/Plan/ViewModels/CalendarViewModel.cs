using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plan.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public Command WeekBackCommand { get; }
        public Command WeekForwardCommand { get; }
        private string _dateRangeLabel;
        public string DateRangeLabel { get => _dateRangeLabel; set { SetProperty(ref _dateRangeLabel, value); } }
        private DateTime CurrentDateRange { get; set; }

        public CalendarViewModel()
        {
            WeekBackCommand = new Command(async () => await ExecuteWeekBackCommand());
            WeekForwardCommand = new Command(async () => await ExecuteWeekForwardCommand());

            List<DateTime> days = GetDaysOfWeek(DateTime.Now);
            CurrentDateRange = days[0];
            DateRangeLabel = DateRangeToString(days);
        }

        private List<DateTime> GetDaysOfWeek(DateTime date)
        {
            date = date.Date;
            int currentDay = (int)date.DayOfWeek;
            DateTime monday = date.AddDays(-currentDay + 1);
            List<DateTime> daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d))
                .ToList();
            return daysThisWeek;
        }

        private string DateRangeToString(List<DateTime> date)
        {
            return date[0].Day + "." + date[0].Month + " - " + date[date.Count - 1].Day + "." + date[date.Count - 1].Month;
        }

        private async Task ExecuteWeekBackCommand()
        {
            CurrentDateRange = CurrentDateRange.AddDays(-7);
            DateRangeLabel = DateRangeToString(GetDaysOfWeek(CurrentDateRange));
        }

        private async Task ExecuteWeekForwardCommand()
        {
            CurrentDateRange = CurrentDateRange.AddDays(7);
            DateRangeLabel = DateRangeToString(GetDaysOfWeek(CurrentDateRange));
        }
    }
}