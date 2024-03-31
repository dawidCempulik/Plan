using Plan.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<int> DeleteItemAsync(CalendarEvent item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
