using Plan.Models;
using Plan.Services;
using Plan.Views;
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
        public Grid WeekGrid { get; set; }
        public Command WeekBackCommand { get; }
        public Command WeekForwardCommand { get; }
        public Command<int> EventTappedCommand { get; }

        private string _dateRangeLabel;
        public string DateRangeLabel { get => _dateRangeLabel; set { SetProperty(ref _dateRangeLabel, value); } }
        private DateTime CurrentDateRange { get; set; }
        private List<CalendarEvent> SelectedEvents { get; set; }
        private List<StackLayout> PageItemsList { get; set; }
        private List<StackLayout> LocalPageItemsList { get; set; }

        public CalendarViewModel()
        {
            WeekBackCommand = new Command(async () => await ExecuteWeekBackCommand());
            WeekForwardCommand = new Command(async () => await ExecuteWeekForwardCommand());
            EventTappedCommand = new Command<int>(ExecuteEventTappedCommand);

            List<DateTime> days = GetDaysOfWeek(DateTime.Now);
            CurrentDateRange = days[0];
            DateRangeLabel = DateRangeToString(days);

            PageItemsList = new List<StackLayout>();
            LocalPageItemsList = new List<StackLayout>();
        }

        private List<DateTime> GetDaysOfWeek(DateTime date)
        {
            date = date.Date;
            int currentDayOfWeek = ((int)date.DayOfWeek + 6) % 7;
            DateTime monday = date.AddDays(-currentDayOfWeek);
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
            await UpdateEventsList();
        }

        private async Task ExecuteWeekForwardCommand()
        {
            CurrentDateRange = CurrentDateRange.AddDays(7);
            await UpdateEventsList();
        }

        private async Task UpdateEventsList()
        {
            DateRangeLabel = DateRangeToString(GetDaysOfWeek(CurrentDateRange));
            await LoadEvents();
        }

        public async Task LoadEvents()
        {
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;
            DateTime endOfRange = CurrentDateRange.AddDays(8).AddMilliseconds(-1);
            SelectedEvents = await database.GetItemsByDateRange(CurrentDateRange, endOfRange);
            LocalPageItemsList.Clear();

            foreach (CalendarEvent item in SelectedEvents)
            {
                for (int i = 0; i < 7; i ++)
                {
                    if (item.Repeat.Length == 0)
                    {
                        if (Utils.DateRangeOverlap(item.DateTimeStart, item.DateTimeEnd, CurrentDateRange.AddDays(i), CurrentDateRange.AddDays(i + 1).AddMilliseconds(-1))) 
                        {
                            ProcessEvent(item, item.DateTimeStart, item.DateTimeEnd, CurrentDateRange.AddDays(i));
                        }
                        continue;
                    }

                    foreach (char c in item.Repeat)
                    {
                        int dayOfWeek = c - '0';

                        int daysToAdd = -((int)CurrentDateRange.DayOfWeek - (int)dayOfWeek) + 1;
                        DateTime newStart = CurrentDateRange.Date.Add(item.DateTimeStart.TimeOfDay).AddDays(daysToAdd);
                        daysToAdd = (int)(item.DateTimeEnd.Date - item.DateTimeStart.Date).TotalDays;
                        DateTime newEnd = newStart.Date.AddDays(daysToAdd).Add(item.DateTimeEnd.TimeOfDay);

                        if (Utils.DateRangeOverlap(newStart, newEnd, CurrentDateRange.AddDays(i), CurrentDateRange.AddDays(i + 1).AddMilliseconds(-1)))
                        {
                            ProcessEvent(item, newStart, newEnd, CurrentDateRange.AddDays(i));
                        }
                    }
                }
            }

            UpdateGrid();
        }

        private void ProcessEvent(CalendarEvent item, DateTime start, DateTime end, DateTime date)
        {
            DateTime endOfDate = date.AddDays(1).AddMilliseconds(-1);

            int gridRow = ((int)date.DayOfWeek + 6) % 7 + 1;
            int gridColumn = start.Hour + 1;
            int gridColumnSpan = end.Hour - start.Hour;
            if (end.Minute > 0) gridColumnSpan += 1;


            if (gridColumnSpan == 0) gridColumnSpan = 1;

            if (start <= date)
            {
                gridColumn = 1;
            }
            if (end >= endOfDate)
            {
                gridColumnSpan = 24 - gridColumn;
            }

            StackLayout stack = new StackLayout()
            {
                BackgroundColor = Color.Accent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness((float)start.Minute / 60 * 88 + 5, 5, (float)end.Minute / 60 * 88 + 5, 5),
            };

            stack.Children.Add(new Label
            {
                Text = item.Text,
                TextColor = Color.White,
                FontSize = 17
            });

            stack.Children.Add(new Label
            {
                Text = item.Description,
                TextColor = Color.White,
            });

            stack.SetValue(Grid.RowProperty, gridRow);
            stack.SetValue(Grid.ColumnProperty, gridColumn);
            stack.SetValue(Grid.ColumnSpanProperty, gridColumnSpan);

            stack.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 2,
                Command = EventTappedCommand,
                CommandParameter = item.Id
            });

            LocalPageItemsList.Add(stack);

        }

        private void UpdateGrid()
        {
            Console.WriteLine("Nowa ilosc: " + LocalPageItemsList.Count);

            foreach (StackLayout item in PageItemsList)
            {
                WeekGrid.Children.Remove(item);
            }

            foreach (StackLayout item in LocalPageItemsList)
            {
                WeekGrid.Children.Add(item);
            }

            PageItemsList = LocalPageItemsList;
        }

        private async void ExecuteEventTappedCommand(int id)
        {
            if (id == 0)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(EventCreatorPage)}?{nameof(EventCreatorViewModel.ItemId)}={id}");
        }
    }
}