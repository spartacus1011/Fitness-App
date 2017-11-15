﻿using System;
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
        private readonly string exerciseTableDefinition = "";

        private SQLiteConnection connection;

        public DataStore(string dbPath)
        {


            //setup string helper variables
            ExerciseItem exerciseItem = new ExerciseItem();
            List<string> exerciseTableDataList = new List<string>()
            {
                nameof(exerciseItem.GUIDID) + ", ",
                nameof(exerciseItem.ExerciseName) + ", ",
                nameof(exerciseItem.ExerciseTypeId) + ", ",
                nameof(exerciseItem.RequiredReps) + ", ",
                nameof(exerciseItem.RequiredTime) + ", ",
                nameof(exerciseItem.RequiredSets) + ", ",
                nameof(exerciseItem.RequiredSetsCount) + ", ",
                nameof(exerciseItem.DueTime) + ", ", //stored as string in UTC time
                nameof(exerciseItem.IsUsedInRoster) + "", //last one doesnt need a comma
                //Make sure to add in all the others. Remember, there is alot
            };
            foreach (var thing in exerciseTableDataList)
            {
                allExercisesTableData += thing;
            }
            List<string> exerciseTableDefinitionList = new List<string>()
            {
                nameof(exerciseItem.GUIDID) + " string,",
                nameof(exerciseItem.ExerciseName) + " string," ,
                nameof(exerciseItem.ExerciseTypeId) + " int," ,
                nameof(exerciseItem.RequiredReps) + " int,",
                nameof(exerciseItem.RequiredTime) + " int, ",
                nameof(exerciseItem.RequiredSets) + " int, ",
                nameof(exerciseItem.RequiredSetsCount) + " int, ",
                nameof(exerciseItem.DueTime) + " string,", //stored as string in UTC time
                nameof(exerciseItem.IsUsedInRoster) + " bool"  ,//last one doesnt need a comma
                //Make sure to add in all the others. Remember, there is alot
            };
            foreach (var thing in exerciseTableDefinitionList)
            {
                exerciseTableDefinition += thing;
            }
            if(exerciseTableDefinitionList.Count != exerciseTableDataList.Count) //to help me find any mistakes that i make
                throw new Exception("Error: Exercise table data and definitions dont match");


            if (!File.Exists(dbPath))
            {
                connection = DatabaseHelper.ConnectToDatabase(dbPath);
                CreateNewExerciseTable();
                //CreateNewHistoryTable(connection);
            }
            connection = DatabaseHelper.ConnectToDatabase(dbPath);

        }

        public void DisconnectFromDatabase()
        {
            DatabaseHelper.DisconnectFromDatabase(connection);
        }

        public void CreateNewExerciseTable()
        {
            DatabaseHelper.CreateTable(connection, allExercisesTableName, exerciseTableDefinition);
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
            };

            DatabaseHelper.UpdateItem(connection, allExercisesTableName, allExercisesTableData, objectsToWrite);
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

        public List<ExerciseItem> LoadAllExerciseItems()
        {
            DataTable dt = DatabaseHelper.LoadItems(connection, allExercisesTableName, allExercisesTableData);

            ExerciseItem nameExerciseItem = new ExerciseItem();

            List<ExerciseItem> loadedItems= (from rw in dt.AsEnumerable()
                select new ExerciseItem
                {
                    GUIDID = Convert.ToString(rw[nameof(nameExerciseItem.GUIDID)]),
                    ExerciseName = Convert.ToString(rw[nameof(nameExerciseItem.ExerciseName)]),
                    ExerciseTypeId = Convert.ToInt32(rw[nameof(nameExerciseItem.ExerciseTypeId)]),
                    DueTime = Convert.ToDateTime(rw[nameof(nameExerciseItem.DueTime)]),
                    IsUsedInRoster = Convert.ToBoolean(rw[nameof(nameExerciseItem.IsUsedInRoster)]),
                }).ToList();

            return loadedItems;
        }


    }
}