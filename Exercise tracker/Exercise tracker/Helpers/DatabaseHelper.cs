using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_tracker.Classes
{
    public static class DatabaseHelper 
    {

        public static bool CreateDatabase(string filePathAndName) //not really needed unless i plan to create the database but not use it
        {
            if (!File.Exists(filePathAndName))
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

        public static void CreateTable(SQLiteConnection connection, string tableName, string tableDefinition)
        {
            try
            {
                string createCommand = String.Format("create table {0} ({1})", tableName, tableDefinition);
                SQLiteCommand create = new SQLiteCommand(createCommand, connection);
                create.ExecuteNonQuery();
                create.Dispose(); //putting the dispose inside the try seems like a bad idea but how else would you do it?
            }
            catch (SQLiteException e)
            {
                //this usually. Ussually... means that the table that is trying to be created already exists
                Console.WriteLine(e);
            }
        }

        public static void AddItem(SQLiteConnection connection, string tableName, string tableData, List<object> allItems)
        {
            //the order of the items in all items is important!!! it must match the order of things in table data
            //tableData more or less means columns
            tableData = tableData.Replace(" ", ""); //remove all spaces (Not really needed if you goven over the entered string but this makes things a bit neater)
            string[] splitData = tableData.Split(',');

            if (splitData.Length != allItems.Count)
            {
                throw new Exception("Table data length and number of items being added must match"); //consider a console out and return?
            }

            string dataWithAt = "";
            foreach (var thing in splitData)
            {
                dataWithAt += "@" + thing + ", ";
            }
            dataWithAt = dataWithAt.Substring(0, dataWithAt.LastIndexOf(",")); //remove the last comma

            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.Transaction = transaction;

                    string commandString = String.Format("insert into {0} ({1}) values ({2})", tableName, tableData, dataWithAt);
                    command.CommandText = commandString;

                    for (int i = 0; i < splitData.Length; i++)
                    {
                        command.Parameters.AddWithValue(splitData.ElementAt(i), allItems.ElementAt(i));
                    }
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        private static void AddMultipleItems(SQLiteConnection connection, string tableName, string tableData, IEnumerable<IEnumerable<object>> allItems) //an enumberable of an enumerable kinda seems like a bad idea...
        {
            //the order of the items in all items is important!!! it must match the order of things in table data
            //tableData more or less means columns
            tableData = tableData.Replace(" ", ""); //remove all spaces (Not really needed if you goven over the entered string but this makes things a bit neater)
            string[] splitData = tableData.Split(',');

            foreach (var items in allItems)
            {
                if (splitData.Length != items.Count())
                {
                    throw new Exception("Table data length and number of items being added must match"); //consider a console out and return?
                }
            }

            string dataWithAt = "";
            foreach (var thing in splitData)
            {
                dataWithAt += "@" + thing + ", ";
            }
            dataWithAt = dataWithAt.Substring(0, dataWithAt.LastIndexOf(",")); //remove the last comma


            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.Transaction = transaction;

                    foreach (var items in allItems)
                    {
                        string commandString = String.Format("insert into {0} ({1}) values ({2})", tableName, tableData, dataWithAt);
                        command.CommandText = commandString;

                        for (int i = 0; i < splitData.Length; i++)
                        {
                            command.Parameters.AddWithValue(splitData.ElementAt(i), items.ElementAt(i));
                        }
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public static void UpdateItem(SQLiteConnection connection, string tableName, string tableData, List<object> allItems, string whereConditions = "")
        {
            //tableData more or less means columns
            tableData = tableData.Replace(" ", ""); //remove all spaces (Not really needed if you goven over the entered string but this makes things a bit neater)
            string[] splitData = tableData.Split(',');

            if (splitData.Length != allItems.Count())
            {
                throw new Exception("Table data length and number of items being added must match"); //consider a console out and return?
            }

            string dataWithAt = "";
            foreach (var thing in splitData)
            {
                dataWithAt += thing + " = @" + thing + ", ";
            }
            dataWithAt = dataWithAt.Substring(0, dataWithAt.LastIndexOf(",")); //remove the last comma

            if (whereConditions == "") //take care when using this method. Maybe make it good practise to make the ID for all data first just incase. If you are trying to update what you are using in the where conditions, it wont work!!
            {
                whereConditions = splitData.FirstOrDefault() + " = @" + splitData.FirstOrDefault();
            }

            SQLiteTransaction transaction = connection.BeginTransaction();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.Transaction = transaction;

            command.CommandText = String.Format("Update {0} set {1} where {2}", tableName, dataWithAt, whereConditions);

            for (int i = 0; i < splitData.Length; i++)
            {
                command.Parameters.AddWithValue(splitData.ElementAt(i), allItems.ElementAt(i));
            }
            command.ExecuteNonQuery();

            transaction.Commit();
            command.Dispose();
            transaction.Dispose();
        }

        public static DataTable LoadItems(SQLiteConnection connection, string tableName, string tableData, string whereConstraints = "")
        {
            //make sure if you use the where constraints, add the "where " to the string

            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            adapter.SelectCommand = new SQLiteCommand(connection);
            adapter.SelectCommand.CommandText = string.Format("select {0} from {1} {2}", tableData, tableName, whereConstraints);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

    }
}
