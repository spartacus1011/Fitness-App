using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MvvmDialogs;
using Exercise_tracker.Helpers;
using Exercise_tracker.Classes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Exercise_tracker.ViewModels
{
    public class HistoryPageViewModel : ObservableObject, IModalDialogViewModel
    {
        public SeriesCollection GraphData { get; set; } 
        public List<string> XLabels { get; set; }
        //public List<object> GraphsToShow { get; set; } //List of object seems kinda bad. See if theres a better way
        public Dictionary<string,object> AvailableGraphs { get; set; } //should probably be a cast object
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public bool XSeperatorEnabled { get; set; }

        private KeyValuePair<string, object> selectedGraph;
        public KeyValuePair<string, object> SelectedGraph {
            get { return selectedGraph; }
            set {
                if (value.Key != selectedGraph.Key)
                {
                    selectedGraph = value;

                    if (selectedGraph.Key == "All Exercise Counts")
                    {
                        ShowAllExerciseCounts();
                    }
                    else
                    {
                        ShowExerciseHistory(selectedGraph.Value as ExerciseItem); //kinda dangerous but as long as we pay attention to what we add, it should be alright
                    }

                }

            }
        } 
        private bool? dialogResult;
        
        private List<ExerciseItem> AllExerciseItems;
        private DataStore datastore;

        public HistoryPageViewModel (DataStore dataStore)
        {
            AllExerciseItems = dataStore.LoadAllExerciseItems(); //probs doesnt need to be loaded again, but it makes the constructor simpler
            this.datastore = dataStore;

            AvailableGraphs = new Dictionary<string, object>();

            foreach (var exercise in AllExerciseItems)
            {
                AvailableGraphs.Add(exercise.ExerciseName,exercise);
            }
            AvailableGraphs.Add("All Exercise Counts",null);

            //ShowExerciseHistory(AllExerciseItems.FirstOrDefault());
            ShowAllExerciseCounts();
            selectedGraph = new KeyValuePair<string, object>("All Exercise Counts", null);
        }

        //Each type of show graph function will have to set up the graph on its own
        private void ShowExerciseHistory(ExerciseItem exercise)
        {
            //for this mode, we need to setup the graph so that it will display a fixed set of days on the graph(scatter plot)
            XAxisTitle = exercise.ExerciseName;
            YAxisTitle = "Day of the Year completed (This needs to change)";
            GraphData = new SeriesCollection();

            GraphData.Add(new ScatterSeries()
            {
                Values = new ChartValues<int>()
            });

            XLabels = new List<string>();
            XSeperatorEnabled = true;

            List<ExerciseHistoryItem> allHistory = datastore.LoadExerciseHistory(exercise);

            allHistory = allHistory.OrderBy(x => x.TimeCompleted).ToList();

            //DateTime dateStart = allHistory.FirstOrDefault().TimeCompleted;
            //DateTime dateEnd = allHistory.LastOrDefault().TimeCompleted;

            //while (dateStart.DayOfYear != dateEnd.DayOfYear) //this will obviously not work when the year changes
            //{
            //    XLabels.Add(dateStart.ToShortDateString());
            //    dateStart += new TimeSpan(1,0,0,0);
            //}

            foreach (var his in allHistory)
            {
                XLabels.Add(his.TimeCompleted.ToShortDateString());
                GraphData[0].Values.Add(his.TimeCompleted.DayOfYear);
            }

            VisualUpdateGraph();
        }

        private void ShowAllExerciseCounts()
        {
            XAxisTitle = "Exercises";
            YAxisTitle = "Count";
            GraphData = new SeriesCollection();

            GraphData.Add(new ColumnSeries());
            GraphData[0].Values = new ChartValues<int>();

            XSeperatorEnabled = false;

            XLabels = new List<string>();

            foreach (var exerciseItem in AllExerciseItems)
            {
                List<ExerciseHistoryItem> history = datastore.LoadExerciseHistory(exerciseItem);

                GraphData[0].Values.Add(history.Count);
                XLabels.Add(exerciseItem.ExerciseName);
            }

            VisualUpdateGraph();
        }

        public bool? DialogResult
        {
            get { return dialogResult; }
            private set //this should do the same as the other thing
            {
                dialogResult = value;
                RaisePropertyChangedEvent("DialogResult");
            }
        }

        private void VisualUpdateGraph()
        {
            RaisePropertyChangedEvent("GraphData");
            RaisePropertyChangedEvent("XAxisTitle");
            RaisePropertyChangedEvent("YAxisTitle");
            RaisePropertyChangedEvent("XLabels");
            RaisePropertyChangedEvent("XSeperatorEnabled");
        }

    }
}
