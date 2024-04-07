using Plan.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;

namespace Plan.Services
{
    public class CalendarEventsDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<CalendarEventsDatabase> Instance = new AsyncLazy<CalendarEventsDatabase>(async () =>
        {
            var instance = new CalendarEventsDatabase();
            CreateTableResult result = await Database.CreateTableAsync<CalendarEvent>();
            return instance;
        });

        public CalendarEventsDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<CalendarEvent>> GetItemsAsync()
        {
            return Database.Table<CalendarEvent>().ToListAsync();
        }

        public Task<CalendarEvent> GetItemAsync(int id)
        {
            return Database.Table<CalendarEvent>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(CalendarEvent item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> InsertMultipleAsync(IEnumerable<CalendarEvent> items)
        {
            return Database.InsertAllAsync(items);
        }

        public Task<int> DeleteItemAsync(CalendarEvent item)
        {
            return Database.DeleteAsync(item);
        }

        public async Task PurgeAllAsync()
        {
            await Database.DropTableAsync<CalendarEvent>();
            await Database.CreateTableAsync<CalendarEvent>();
        }

        public async Task<List<CalendarEvent>> GetItemsByDateRange(DateTime start, DateTime end)
        {
            List<CalendarEvent> items = await GetItemsAsync();
            List<CalendarEvent> result = new List<CalendarEvent>();

            int startDayOfWeek = ((int)start.DayOfWeek + 6) % 7;
            int daysSpan = (int)(end - start).TotalDays;
            int endDayOfWeek = (startDayOfWeek + daysSpan) % 7;

            foreach (CalendarEvent item in items)
            {
                if (Utils.DateRangeOverlap(item.DateTimeStart, item.DateTimeEnd, start, end))
                {
                    result.Add(item);
                    continue;
                }

                // repeats
                if (item.Repeat.Length > 0)
                {
                    foreach (char c in item.Repeat)
                    {
                        int dayOfWeek = c - '0';

                        int daysToAdd = -((int)end.DayOfWeek - (int)dayOfWeek) + 1;
                        DateTime newStart = end.Date.Add(item.DateTimeStart.TimeOfDay).AddDays(daysToAdd);
                        daysToAdd = (int)(item.DateTimeEnd.Date - item.DateTimeStart.Date).TotalDays;
                        DateTime newEnd = newStart.Date.AddDays(daysToAdd).Add(item.DateTimeEnd.TimeOfDay);

                        if (Utils.DateRangeOverlap(newStart, newEnd, start, end))
                        {
                            result.Add(item);
                            break;
                        }
                    }
                }
            }

            result = result.OrderBy(item => item.DateTimeStart.TimeOfDay).ToList();

            return result;
        }
    }
}
