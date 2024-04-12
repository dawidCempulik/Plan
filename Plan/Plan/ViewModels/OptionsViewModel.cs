using Plan.Models;
using Plan.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Plan.ViewModels
{
    public class OptionsViewModel : BaseViewModel
    {
        public Command ImportCommand { get; }
        public Command ExportCommand { get; }
        public Command PurgeCommand { get; }

        public OptionsViewModel()
        {
            ImportCommand = new Command(async () => await ExecuteImportCommand());
            ExportCommand = new Command(async () => await ExecuteExportCommand());
            PurgeCommand = new Command(async () => await ExecutePurgeCommand());
        }

        private async Task ExecuteImportCommand()
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Jesteś pewny?", "Ta akcja usunie nadpisze istniejące wydarzenia tymi z pliku.", "Tak", "Nie");
            if (!answer) { return; }

            FilePickerFileType customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/json" } },
            });

            PickOptions options = new PickOptions
            {
                PickerTitle = "Wybierz plik z danymi",
                FileTypes = customFileType,
            };

            FileResult result = await PickFile(options);

            if (result == null) return;


            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;

            string jsonString = File.ReadAllText(result.FullPath);
            List<CalendarEvent> events = JsonSerializer.Deserialize<List<CalendarEvent>>(jsonString);

            await database.PurgeAllAsync();
            await database.InsertMultipleAsync(events);

            await App.Current.MainPage.DisplayAlert("Importowanie", "Zimportowano dane.", "Ok");
        }

        private async Task ExecuteExportCommand()
        {
            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;

            string fileLocation = "/storage/emulated/0/Documents/Plan";
            string fileName = "calendarEvents.json";
            string filePath = Path.Combine(fileLocation, fileName);

            string jsonString = JsonSerializer.Serialize(await database.GetItemsAsync());
            Directory.CreateDirectory(fileLocation);
            File.WriteAllText(filePath, jsonString);

            await App.Current.MainPage.DisplayAlert("Eksportowanie", "Wyeksportowano dane. Możesz je znaleźć w Dokumenty/Plan/calendarEvents.json", "Ok");
        }

        private async Task ExecutePurgeCommand()
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Jesteś pewny?", "Ta akcja usunie WSZYSTKIE wydarzenia.", "Tak", "Nie");
            if (!answer) { return; }

            CalendarEventsDatabase database = await CalendarEventsDatabase.Instance;
            await database.PurgeAllAsync();
        }

        private async Task<FileResult> PickFile(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);

                return result;
            }
            catch
            {
                // The user canceled or something went wrong
            }

            return null;
        }
    }
}
