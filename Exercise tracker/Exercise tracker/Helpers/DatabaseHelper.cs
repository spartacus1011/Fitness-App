using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace Exercise_tracker.Helpers
{
    /// <summary>
    /// Use this class to greatly simplify the interactions between the code and the database
    /// </summary>
    public static class DatabaseHelper
    {
        public static bool CreateDatabase(string filePathAndName)
        {
            if (!System.IO.File.Exists(filePathAndName))
            {
                Console.WriteLine("Creating Database at: " + filePathAndName);
                SQLiteConnection.CreateFile(filePathAndName);
                return true;
            }
            else
            {
                Console.WriteLine("Failed to create new database: Database already exists");
                return false;
            }
        }

    }
}
