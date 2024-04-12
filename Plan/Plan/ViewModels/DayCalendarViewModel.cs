using Plan.Models;
using Plan.Services;
using Plan.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static SQLite.SQLite3;

namespace Plan.ViewModels
{
    public class DayCalendarViewModel : BaseViewModel
    {
        public Command DayBackCommand { get; }
        public Command DayForwardCommand { get; }
        public Command AddItemCommand { get; }
        public Command<DayCalendarEventPageItem> EventTappedCommand { get; }

        private string _dateLabel;
        public string DateLabel { get => _dateLabel; set { SetProperty(ref _dateLabel, value); } }
        private DateTime CurrentDate { get; set; }

        public ObservableCollection<DayCalendarEventPageItem> CalendarEventsPageList { private set; get; }
        private List<CalendarEvent> SelectedEvents { get; set; }

        public DayCalendarViewModel()
        {
            CalendarEventsPageList = new ObservableCollection<DayCalendarEventPageItem>();

            DayBackCommand = new Command(async () => await ExecuteDayBackCommand());
            DayForwardCommand = new Command(async () => await ExecuteDayForwardCommand());
            AddItemCommand = new Command(async () => await ExecuteAddItemCommand());
            EventTappedCommand = new Command<DayCalendarEventPageItem>(ExecuteEventTappedCommand);

            CurrentDate = DateTime.Now.Date;
            DateLabel = DateToString(DateTime.Now);
        }

        private string DateToString(DateTime date)
        {
            string day1 = (date.Day < 10 ? "0" : "") + date.Day;
            string month1 = (date.Month < 10 ? "0" : "") + date.Month;

            return day1 + "." + month1 + " " + Constants.daysOfWeek[((int)date.DayOfWeek + 6) % 7];
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
            await LoadEvents();
        }

        public async Task LoadEvents()
        {
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;
            List<DayCalendarEventPageItem> newPageList = new List<DayCalendarEventPageItem>();
            CalendarEventsPageList.Clear();

            DateTime endOfCurrentDate = CurrentDate.AddDays(1).AddMilliseconds(-1);
            SelectedEvents = await database.GetItemsByDateRange(CurrentDate, endOfCurrentDate);

            if (SelectedEvents.Count == 0)
            {
                CalendarEventsPageList.Add(new DayCalendarEventPageItem(0, "Nie ma wydarzeń tego dnia", "", "", ""));
            }

            foreach (CalendarEvent item in SelectedEvents)
            {
                string timeLabel1 = TimeToString(item.DateTimeStart);
                string timeLabel2 = TimeToString(item.DateTimeEnd);

                if (item.Repeat.Length > 0)
                {
                    foreach (char c in item.Repeat)
                    {
                        timeLabel1 = TimeToString(item.DateTimeStart);
                        timeLabel2 = TimeToString(item.DateTimeEnd);

                        int dayOfWeek = c - '0';

                        int daysToAdd = -((int)CurrentDate.DayOfWeek - (int)dayOfWeek) + 1;
                        DateTime newStart = CurrentDate.Date.Add(item.DateTimeStart.TimeOfDay).AddDays(daysToAdd);
                        daysToAdd = (int)(item.DateTimeEnd.Date - item.DateTimeStart.Date).TotalDays;
                        DateTime newEnd = newStart.Date.AddDays(daysToAdd).Add(item.DateTimeEnd.TimeOfDay);

                        if (Utils.DateRangeOverlap(newStart, newEnd, CurrentDate, endOfCurrentDate))
                        {
                            if (newStart <= CurrentDate)
                            {
                                timeLabel1 = "Start";
                            }
                            if (newEnd >= endOfCurrentDate)
                            {
                                timeLabel2 = "Koniec";
                            }
                            if (newStart <= CurrentDate && newEnd >= endOfCurrentDate) {
                                timeLabel1 = "Cały";
                                timeLabel2 = "Dzień";
                            }

                            DayCalendarEventPageItem newItem = new DayCalendarEventPageItem(item.Id, item.Text, item.Description, timeLabel1, timeLabel2);
                            newPageList.Add(newItem);
                        }
                    }
                }
                else
                {
                    if (item.DateTimeStart <= CurrentDate)
                    {
                        timeLabel1 = "Start";
                    }
                    if (item.DateTimeEnd >= endOfCurrentDate)
                    {
                        timeLabel2 = "Koniec";
                    }
                    if (item.DateTimeStart <= CurrentDate && item.DateTimeEnd >= endOfCurrentDate)
                    {
                        timeLabel1 = "Cały";
                        timeLabel2 = "Dzień";
                    }

                    DayCalendarEventPageItem newItem = new DayCalendarEventPageItem(item.Id, item.Text, item.Description, timeLabel1, timeLabel2);
                    newPageList.Add(newItem);
                }
            }

            newPageList = newPageList.OrderByDescending(item => item.TimeStartLabel).ToList();

            int wholeDayCount = 0;
            int startDayCount = 0;
            foreach (DayCalendarEventPageItem item in newPageList)
            {
                if (item.TimeStartLabel == "Cały")
                {
                    CalendarEventsPageList.Insert(0, item);
                    wholeDayCount += 1;
                }
                else if (item.TimeStartLabel == "Start")
                {
                    CalendarEventsPageList.Insert(wholeDayCount, item);
                    startDayCount += 1;
                }
                else if (item.TimeEndLabel == "Koniec")
                {
                    CalendarEventsPageList.Add(item);
                }
                else
                {
                    CalendarEventsPageList.Insert(wholeDayCount + startDayCount, item);
                }
            }

            OnPropertyChanged(nameof(CalendarEventsPageList));
        }

        private string TimeToString(DateTime date)
        {
            string hour = (date.Hour < 10 ? "0" : "") + date.Hour;
            string minute = (date.Minute < 10 ? "0" : "") + date.Minute;

            return hour + ":" + minute;
        }

        private async void ExecuteEventTappedCommand(DayCalendarEventPageItem item)
        {
            if (item == null)
                return;
            if (item.Id == 0)
                return;

            await Shell.Current.GoToAsync($"{nameof(EventCreatorPage)}?{nameof(EventCreatorViewModel.ItemId)}={item.Id}");
        }
    }
}