using Plan.Models;
using Plan.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plan.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class EventCreatorViewModel : BaseViewModel
    {
        private int itemId;
        private string text;
        private string description;
        private TimeSpan timeStart;
        private DateTime dateStart;
        private TimeSpan timeEnd;
        private DateTime dateEnd;
        private bool[] repeat;

        public EventCreatorViewModel()
        {

            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);

            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;

            repeat = new bool[7];

            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private async Task LoadItemInfo(int id)
        {
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;

            CalendarEvent calendarEvent = await database.GetItemAsync(id);
            Text = calendarEvent.Text;
            Description = calendarEvent.Description;
            TimeStart = calendarEvent.DateTimeStart.TimeOfDay;
            DateStart = calendarEvent.DateTimeStart.Date;
            TimeEnd = calendarEvent.DateTimeEnd.TimeOfDay;
            DateEnd = calendarEvent.DateTimeEnd.Date;
            bool[] newRepeat = new bool[7];
            Console.WriteLine(calendarEvent.Repeat + "   " + calendarEvent.Repeat.Length);
            for (int i = 0; i < calendarEvent.Repeat.Length; i++)
            {
                Console.WriteLine(i + "  " + calendarEvent.Repeat[i]);
                newRepeat[calendarEvent.Repeat[i] - '0'] = true;
            }
            Repeat = newRepeat;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
        }

        public int ItemId
        {
            get => itemId;
            set
            {
                if (value != 0)
                {
                    SetProperty(ref itemId, value);
                    Title = "Edytowanie wydarzenia";
                    _ = LoadItemInfo(ItemId);
                }
            }
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public TimeSpan TimeStart
        {
            get => timeStart;
            set => SetProperty(ref timeStart, value);
        }

        public DateTime DateStart
        {
            get => dateStart;
            set => SetProperty(ref dateStart, value);
        }

        public TimeSpan TimeEnd
        {
            get => timeEnd;
            set => SetProperty(ref timeEnd, value);
        }

        public DateTime DateEnd
        {
            get => dateEnd;
            set => SetProperty(ref dateEnd, value);
        }

        public bool[] Repeat
        {
            get => repeat;
            set => SetProperty(ref repeat, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            string repeatString = "";
            for (int i = 0; i < Repeat.Length; i++)
            {
                bool value = Repeat[i];
                if (value)
                {
                    repeatString += i;
                }
            }

            CalendarEvent calendarEvent = new CalendarEvent(ItemId, Text, Description, DateStart.Date.Add(TimeStart), DateEnd.Date.Add(TimeEnd), repeatString);
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;

            await database.SaveItemAsync(calendarEvent);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
