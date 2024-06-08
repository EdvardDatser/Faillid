using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace Faillid.Models
{
    public class FriendRepository
    {
        SQLiteConnection database;

        public FriendRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Friend>();
        }

        public IEnumerable<Friend> GetItems()
        {
            return database.Table<Friend>().ToList();
        }

        public Friend GetItem(int id)
        {
            return database.Get<Friend>(id);
        }

        public int DeleteItem(int id)
        {
            return database.Delete<Friend>(id);
        }

        public int SaveItem(Friend item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}
