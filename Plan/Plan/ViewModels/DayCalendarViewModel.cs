using Plan.Models;
using Plan.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

/*
 * TODO: Add a page for adding events and make the item cell as it actually should be
 */

namespace Plan.ViewModels
{
    public class DayCalendarViewModel : BaseViewModel
    {
        public Command DayBackCommand { get; }
        public Command DayForwardCommand { get; }

        private string _dateLabel;
        public string DateLabel { get => _dateLabel; set { SetProperty(ref _dateLabel, value); } }
        private DateTime CurrentDate { get; set; }

        public ObservableCollection<CalendarEventPageItem> CalendarEventsPageList { private set; get; }
        private List<CalendarEvent> SelectedEvents {  get; set; }

        public DayCalendarViewModel()
        {
            CalendarEventsPageList = new ObservableCollection<CalendarEventPageItem>();

            DayBackCommand = new Command(async () => await ExecuteDayBackCommand());
            DayForwardCommand = new Command(async () => await ExecuteDayForwardCommand());

            CurrentDate = DateTime.Now.Date;
            DateLabel = DateTime.Now.ToShortDateString();

            _ = LoadEventsByDay(CurrentDate);
        }

        private async Task ExecuteDayBackCommand()
        {
            CurrentDate = CurrentDate.AddDays(-1);
            DateLabel = CurrentDate.ToShortDateString();
            await LoadEventsByDay(CurrentDate);
        }

        private async Task ExecuteDayForwardCommand()
        {
            CurrentDate = CurrentDate.AddDays(1);
            DateLabel = CurrentDate.ToShortDateString();
            await LoadEventsByDay(CurrentDate);
        }

        private async Task LoadEventsByDay(DateTime date)
        {
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;

            Console.WriteLine("Connected (?)");
            CalendarEventsPageList.Clear();
            SelectedEvents = await database.GetItemsAsync();
            Console.WriteLine("Got items (?)");

            foreach (CalendarEvent item in SelectedEvents)
            {
                CalendarEventsPageList.Add(new CalendarEventPageItem(item.Text, item.DateTime.TimeOfDay.ToString()));
            }

            OnPropertyChanged(nameof(CalendarEventsPageList));
            Console.WriteLine("Succeded!");
        }
    }
}