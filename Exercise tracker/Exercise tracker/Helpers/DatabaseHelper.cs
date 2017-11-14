using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Exercise_tracker.Classes;

namespace Exercise_tracker.Helpers
{
    /// <summary>
    /// Use this class to greatly simplify the interactions between the code and the database
    /// </summary>
    public static class DatabaseHelper
    {
        private static string exerciseTableName = "Exercises";
        private static string exerciseTableData = "";
        private static string exerciseTableDefinition = "";

        static DatabaseHelper()
        {
            ExerciseItem exerciseItem = new ExerciseItem(); 
            List<string> exerciseTableDataList = new List<string>()
            {
                nameof(exerciseItem.GUIDID) + ", ",
                nameof(exerciseItem.ExerciseName) + ", ",
                nameof(exerciseItem.ExerciseTypeId) + ", ",
                nameof(exerciseItem.IsUsedInRoster) + ", ",
                //Make sure to add in all the others. Remember, there is alot
            };
            foreach (var thing in exerciseTableDataList)
            {
                exerciseTableData += thing;
            }
            List<string> exerciseTableDefinitionList = new List<string>()
            {
                nameof(exerciseItem.GUIDID) + " string,",
                nameof(exerciseItem.ExerciseName) + " string," ,
                nameof(exerciseItem.ExerciseTypeId) + " int," ,
                nameof(exerciseItem.IsUsedInRoster) + " bool"  ,
                //Make sure to add in all the others. Remember, there is alot
            };
            foreach (var thing in exerciseTableDefinitionList)
            {
                exerciseTableDefinition += thing;
            }
        }

        public static SQLiteConnection ConnectToDatabase(string dbPath)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath + ";" + "Pooling=true;" + "Synchronous=Off;");
            connection.Open();
            return connection;
        }

        public static void DisconnectFromDatabase(SQLiteConnection connection)
        {
            connection.Close();
        }

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

        public static void CreateNewExerciseTable(SQLiteConnection connection)
        {
            CreateTable(connection, exerciseTableName, exerciseTableDefinition);
        }

        private static void CreateTable(SQLiteConnection connection, string tableName, string tableDefinition)
        {
            try
            {
                string createCommand = string.Format("create table {0} ({1})", tableName, tableDefinition);
                SQLiteCommand create = new SQLiteCommand(createCommand, connection);
                create.ExecuteNonQuery();
                create.Dispose(); //putting the dispose inside the try seems like a bad idea but how else would you do it?
            }
            catch (SQLiteException e)
            {
                //this usually means that the table that is trying to be created already exists
                Console.WriteLine(e);
            }

        }

    }
}
