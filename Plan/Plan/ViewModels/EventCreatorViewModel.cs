using Plan.Models;
using Plan.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Plan.ViewModels
{
    public class EventCreatorViewModel : BaseViewModel
    {
        private string text;
        private string description;
        private TimeSpan timeStart;
        private DateTime dateStart;
        private TimeSpan timeEnd;
        private DateTime dateEnd;
        private string repeat;

        public EventCreatorViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);

            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;

            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
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

        public string Repeat
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
            CalendarEvent calendarEvent = new CalendarEvent(0, Text, Description, DateStart.Add(TimeStart), DateEnd.Add(TimeEnd), Repeat);
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;

            await database.SaveItemAsync(calendarEvent);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
