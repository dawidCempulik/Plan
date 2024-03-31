using Plan.Models;
using Plan.Services;
using Plan.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plan.ViewModels
{
    public class DayCalendarViewModel : BaseViewModel
    {
        public Command DayBackCommand { get; }
        public Command DayForwardCommand { get; }
        public Command AddItemCommand { get; }
        public Command<CalendarEventPageItem> EventTapped { get; }

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
            AddItemCommand = new Command(async () => await ExecuteAddItemCommand());
            EventTapped = new Command<CalendarEventPageItem>(ExecuteEventTappedCommand);

            CurrentDate = DateTime.Now.Date;
            DateLabel = DateToString(DateTime.Now);
        }

        private string DateToString(DateTime date)
        {
            string day1 = (date.Day < 10 ? "0" : "") + date.Day;
            string month1 = (date.Month < 10 ? "0" : "") + date.Month;

            return day1 + "." + month1;
        }

        private async Task ExecuteDayBackCommand()
        {
            CurrentDate = CurrentDate.AddDays(-1);
            await UpdateEventsList();
        }

        private async Task ExecuteDayForwardCommand()
        {
            CurrentDate = CurrentDate.AddDays(1);
            await UpdateEventsList();
        }

        private async Task ExecuteAddItemCommand()
        {
            await Shell.Current.GoToAsync(nameof(EventCreatorPage));
        }

        private async Task UpdateEventsList()
        {
            DateLabel = DateToString(CurrentDate);
            await LoadEventsByDay();
        }

        public async Task LoadEventsByDay()
        {
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;

            CalendarEventsPageList.Clear();
            SelectedEvents = new List<CalendarEvent>();

            //SelectedEvents = await database.GetItemsByDateAsync(CurrentDate);
            List<CalendarEvent> events = await database.GetItemsAsync();

            int currentDayOfWeek = (int)CurrentDate.DayOfWeek - 1;
            if (currentDayOfWeek < 0) currentDayOfWeek += 7;

            foreach (CalendarEvent calendarEvent in events)
            {
                if (calendarEvent.DateTimeStart.Date == CurrentDate || calendarEvent.Repeat.Contains(currentDayOfWeek.ToString()))
                {
                    SelectedEvents.Add(calendarEvent);
                }
            }
            
            if (SelectedEvents.Count == 0) 
            {
                CalendarEventsPageList.Add(new CalendarEventPageItem(0, "Nie ma wydarzeń tego dnia", "", "", ""));
            }
            else
            {
                SelectedEvents = SelectedEvents.OrderBy(item => item.DateTimeStart.TimeOfDay).ToList();
            }

            foreach (CalendarEvent item in SelectedEvents)
            {
                CalendarEventsPageList.Add(new CalendarEventPageItem(item.Id, item.Text, item.Description, TimeToString(item.DateTimeStart), TimeToString(item.DateTimeEnd)));
            }

            OnPropertyChanged(nameof(CalendarEventsPageList));
        }

        private string TimeToString(DateTime date)
        {
            string hour = (date.Hour < 10 ? "0" : "") + date.Hour;
            string minute = (date.Minute < 10 ? "0" : "") + date.Minute;

            return hour + ":" + minute;
        }

        private async void ExecuteEventTappedCommand(CalendarEventPageItem item)
        {
            if (item == null)
                return;
            if (item.Id == 0) 
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(EventCreatorPage)}?{nameof(EventCreatorViewModel.ItemId)}={item.Id}");
        }
    }
}