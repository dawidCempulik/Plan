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
            int currentDay = (int)date.DayOfWeek - 1;
            if (currentDay < 0) currentDay += 7;
            DateTime monday = date.AddDays(-currentDay);
            List<DateTime> daysThisWeek = Enumerable.Range(0, 7)
                .Select(d => monday.AddDays(d))
                .ToList();
            return daysThisWeek;
        }

        private string DateRangeToString(List<DateTime> date)
        {
            string day1 = (date[0].Day < 10 ? "0" : "") + date[0].Day;
            string month1 = (date[0].Month < 10 ? "0" : "") + date[0].Month;

            string day2 = (date[date.Count - 1].Day < 10 ? "0" : "") + date[date.Count - 1].Day;
            string month2 = (date[date.Count - 1].Month < 10 ? "0" : "") + date[date.Count - 1].Month;

            return day1 + "." + month1 + " - " + day2 + "." + month2;
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