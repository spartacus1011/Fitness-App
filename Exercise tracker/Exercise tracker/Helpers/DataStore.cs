using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Exercise_tracker.Classes;

namespace Exercise_tracker.Helpers
{
    /// <summary>
    /// I should make this a non static class that way it can hold the connection so that it doesnt need to be seen in the main code AND 
    /// I can still have multiple database connections
    /// </summary>
    public class DataStore
    {
        private readonly string allExercisesTableName = "Exercises";
        private readonly string allExercisesTableData = "";
        private readonly string allexercisesTableDefinition = "";

        private readonly string exerciseHistoryTableName = "Exercise_History";
        private readonly string exerciseHistoryTableData = "";
        private readonly string exerciseHistoryTableDefinition = "";

        private SQLiteConnection connection;

        //dummy items for naming purposes
        private readonly ExerciseItem nameExerciseItem = new ExerciseItem();
        private readonly ExerciseHistoryItem nameHistoryItem = new ExerciseHistoryItem("", false, DateTime.Now); 


        public DataStore(string dbPath, bool fakeDB = false)
        {
            //setup string helper variables
            
            List<string> exerciseTableDataList = new List<string>()
            {
                nameof(nameExerciseItem.GUIDID) + ", ",
                nameof(nameExerciseItem.ExerciseName) + ", ",
                nameof(nameExerciseItem.ExerciseTypeId) + ", ",
                nameof(nameExerciseItem.RequiredReps) + ", ",
                nameof(nameExerciseItem.RequiredTime) + ", ",
                nameof(nameExerciseItem.RequiredSets) + ", ",
                nameof(nameExerciseItem.RequiredSetsCount) + ", ",
                nameof(nameExerciseItem.DueTime) + ", ", //stored as string in UTC time
                nameof(nameExerciseItem.IsUsedInRoster) + ", ", //last one doesnt need a comma
                nameof(nameExerciseItem.MuscleGroupId) + ", ",
                nameof(nameExerciseItem.Weight) + ""
            };
            foreach (var thing in exerciseTableDataList)
            {
                allExercisesTableData += thing;
            }
            List<string> exerciseTableDefinitionList = new List<string>()
            {
                nameof(nameExerciseItem.GUIDID) + " string,",
                nameof(nameExerciseItem.ExerciseName) + " string," ,
                nameof(nameExerciseItem.ExerciseTypeId) + " int," ,
                nameof(nameExerciseItem.RequiredReps) + " int,",
                nameof(nameExerciseItem.RequiredTime) + " int, ",
                nameof(nameExerciseItem.RequiredSets) + " int, ",
                nameof(nameExerciseItem.RequiredSetsCount) + " int, ",
                nameof(nameExerciseItem.DueTime) + " string,", //stored as string in UTC time
                nameof(nameExerciseItem.IsUsedInRoster) + " bool,",//last one doesnt need a comma
                nameof(nameExerciseItem.MuscleGroupId) + " int,",
                nameof(nameExerciseItem.Weight) + " float"
            };
            foreach (var thing in exerciseTableDefinitionList)
            {
                allexercisesTableDefinition += thing;
            }
            if(exerciseTableDefinitionList.Count != exerciseTableDataList.Count) //to help me find any mistakes that i make
                throw new Exception("Error: Exercise table data and definitions dont match");

            List<string> exerciseHistoryTableDataList = new List<string>()
            {
                nameof(nameHistoryItem.ExerciseGUIDID) + ", ",
                nameof(nameHistoryItem.IsRep) + ", ",
                nameof(nameHistoryItem.TimeCompleted) + "",
            };
            foreach (var thing in exerciseHistoryTableDataList)
            {
                exerciseHistoryTableData += thing;
            }
            List<string> exerciseHistoryTableDefinitionList = new List<string>()
            {
                nameof(nameHistoryItem.ExerciseGUIDID) + " string, ",
                nameof(nameHistoryItem.IsRep) + " bool, ",
                nameof(nameHistoryItem.TimeCompleted) + " string",
            };
            foreach (var thing in exerciseHistoryTableDefinitionList)
            {
                exerciseHistoryTableDefinition += thing;
            }


            if (fakeDB)
            {
                connection = DatabaseHelper.ConnectToMemoryDatabase();
                CreateAllNewTables();
            }
            else
            {
                if (!File.Exists(dbPath))
                {
                    connection = DatabaseHelper.ConnectToDatabase(dbPath);
                    CreateAllNewTables();
                }
                else
                {
                    connection = DatabaseHelper.ConnectToDatabase(dbPath);
                }
            }
        }

        private void CreateAllNewTables()
        {
            CreateNewExerciseTable();
            CreateNewHistoryTable();
            //CreateNewSettingsTable();
        }

        public void DisconnectFromDatabase()
        {
            DatabaseHelper.DisconnectFromDatabase(connection);
        }

        private void CreateNewExerciseTable()
        {
            DatabaseHelper.CreateTable(connection, allExercisesTableName, allexercisesTableDefinition);
        }

        private void CreateNewHistoryTable()
        {
            DatabaseHelper.CreateTable(connection, exerciseHistoryTableName, exerciseHistoryTableDefinition);
        }

        public void AddExercise(ExerciseItem exercise)
        {
            List<object> exercisesToAdd = new List<object>()
            {
                exercise.GUIDID,
                exercise.ExerciseName,
                exercise.ExerciseTypeId,
                exercise.RequiredReps,
                exercise.RequiredTime,
                exercise.RequiredSets,
                exercise.RequiredSetsCount,
                exercise.DueTime.ToUniversalTime().ToString("O"),
                exercise.IsUsedInRoster,
                exercise.MuscleGroupId,
                exercise.Weight,
                
            };

            DatabaseHelper.AddItem(connection, allExercisesTableName, allExercisesTableData, exercisesToAdd);
        }

        public void UpdateExercise(ExerciseItem exercise)
        {
            List<object> objectsToWrite = new List<object>
            {
                exercise.GUIDID,
                exercise.ExerciseName,
                exercise.ExerciseTypeId,
                exercise.RequiredReps,
                exercise.RequiredTime,
                exercise.RequiredSets,
                exercise.RequiredSetsCount,
                exercise.DueTime.ToUniversalTime().ToString("O"),
                exercise.IsUsedInRoster,
                exercise.MuscleGroupId,
                exercise.Weight,
            };

            DatabaseHelper.UpdateItem(connection, allExercisesTableName, allExercisesTableData, objectsToWrite);
        }

        public void DeleteExercise(ExerciseItem exercise)
        {
            DatabaseHelper.DeleteItem(connection, allExercisesTableName, nameof(exercise.GUIDID), exercise.GUIDID);
        }

        public void UpdateDueTime(ExerciseItem exercise)
        {
            List<object> objectsToWrite = new List<object>()
            {
                exercise.GUIDID,
                exercise.DueTime.ToUniversalTime().ToString("O"),
            };

            string DueTimeData = nameof(exercise.GUIDID) + ", " + nameof(exercise.DueTime);
            DatabaseHelper.UpdateItem(connection, allExercisesTableName,DueTimeData,objectsToWrite);
        }

        public void UpdateIsUsedInRoster(ExerciseItem exercise)
        {
            List<object> objectsToWrite = new List<object>()
            {
                exercise.GUIDID,
                exercise.IsUsedInRoster,
            };

            string IsUsedInRosterData = nameof(exercise.GUIDID) + ", " + nameof(exercise.IsUsedInRoster);
            DatabaseHelper.UpdateItem(connection, allExercisesTableName, IsUsedInRosterData, objectsToWrite);
        }

        public void AddHistoryItem(ExerciseHistoryItem item)
        {
            List<object> objectsToWrite = new List<object>()
            {
                item.ExerciseGUIDID,
                item.IsRep,
                item.TimeCompleted.ToUniversalTime().ToString("O")
            };
            DatabaseHelper.AddItem(connection, exerciseHistoryTableName, exerciseHistoryTableData, objectsToWrite);
        }

        public List<ExerciseItem> LoadAllExerciseItems()
        {
            //Do version control here
            DataTable dt = null;

            bool loop = true;

            while (loop)
            {
                try
                {
                    dt = DatabaseHelper.LoadItems(connection, allExercisesTableName, allExercisesTableData);
                    loop = false;
                }
                catch (SQLiteException ex)
                {
                    if (ex.Message.Contains("no such column: MuscleGroupId"))
                    {
                        DatabaseHelper.AddColumn(connection, allExercisesTableName, "MuscleGroupId int"); 
                        continue; //try again with new column
                    }
                    else if (ex.Message.Contains("no such column: Weight"))
                    {
                        DatabaseHelper.AddColumn(connection, allExercisesTableName, "Weight float");
                        continue; //try again with new column
                    }
                    else
                    {
                        throw new Exception("Error Loading database, this isnt a migration issue that we have handled");
                    }
                }
            }


            if(dt == null)
                throw new Exception("Something has gone wrong in loading the database");

            foreach (var row in dt.AsEnumerable()) //This loop could get nasty on long iterations. Keep an eye on it
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (row.IsNull(i))
                    {
                        //throw new Exception("One of the datasets are null. Need to manually edit to fix");
                        System.Diagnostics.Debug.WriteLine(row[i] + " was null. Converting to 0");
                        row[i] = 0; //0 is a very cool value because it can be many types. still keep an eye on it
                    }
                }
            }

            List<ExerciseItem> loadedItems = (from rw in dt.AsEnumerable()
                select new ExerciseItem
                {
                    GUIDID = Convert.ToString(rw[nameof(nameExerciseItem.GUIDID)]),
                    ExerciseName = Convert.ToString(rw[nameof(nameExerciseItem.ExerciseName)]),
                    ExerciseTypeId = Convert.ToInt32(rw[nameof(nameExerciseItem.ExerciseTypeId)]),
                    RequiredReps = Convert.ToInt32(rw[nameof(nameExerciseItem.RequiredReps)]),
                    RequiredTime = Convert.ToInt32(rw[nameof(nameExerciseItem.RequiredTime)]),
                    RequiredSets = Convert.ToInt32(rw[nameof(nameExerciseItem.RequiredSets)]),
                    RequiredSetsCount = Convert.ToInt32(rw[nameof(nameExerciseItem.RequiredSetsCount)]),
                    DueTime = Convert.ToDateTime(rw[nameof(nameExerciseItem.DueTime)]),
                    IsUsedInRoster = Convert.ToBoolean(rw[nameof(nameExerciseItem.IsUsedInRoster)]),
                    MuscleGroupId = Convert.ToInt32(rw[nameof(nameExerciseItem.MuscleGroupId)]),
                    Weight = Convert.ToSingle(rw[nameof(nameExerciseItem.Weight)]),
                }).ToList();

            return loadedItems;
        }



        public List<ExerciseHistoryItem> LoadExerciseHistory(ExerciseItem exercise)
        {
            string guidname = nameof(nameHistoryItem.ExerciseGUIDID);
            
            DataTable dt = DatabaseHelper.LoadItems(connection, exerciseHistoryTableName, exerciseHistoryTableData, new List<string>(){guidname}, new List<object>(){exercise.GUIDID});

            List<ExerciseHistoryItem> history= (from rw in dt.AsEnumerable()
                select new ExerciseHistoryItem(
                    Convert.ToString(rw[nameof(nameHistoryItem.ExerciseGUIDID)]), 
                    Convert.ToBoolean(rw[nameof(nameHistoryItem.IsRep)]), 
                    Convert.ToDateTime(rw[nameof(nameHistoryItem.TimeCompleted)]))
                ).ToList();

            return history;
        }
    }
}
