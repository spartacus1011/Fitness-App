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
        public SeriesCollection GraphData { get; set; } //i think this needs to stay public so the graph can see it
        public List<string> XLabels { get; set; }
        public Separator Seperator { get; set; }

        private bool? dialogResult;
        
        private List<ExerciseItem> AllExerciseItems;
        private DataStore datastore;

        public HistoryPageViewModel (DataStore dataStore)
        {
            AllExerciseItems = dataStore.LoadAllExerciseItems(); //probs doesnt need to be done again, but it makes the constructor simpler
            this.datastore = dataStore;

            

            //Random rng = new Random();

            //TestData.Add(new LineSeries()
            //{
            //    Values = new ChartValues<ObservableValue>()
            //});

            //for (int i = 0; i < 100; i++)
            //{
            //    TestData[0].Values.Add(rng.Next());
            //    //TestData[1].Values.Add(new ObservableValue(rng.Next()));
            //}
            //ShowExerciseHistory(AllExerciseItems.FirstOrDefault());
            
            ShowAllExerciseCounts();
        }

        //Each type of show graph function will have to set up the graph on its own
        private void ShowExerciseHistory(ExerciseItem exercise)
        {
            //for this mode, we need to setup the graph so that it will display a fixed set of days on the graph(scatter plot)

            GraphData = new SeriesCollection();

            GraphData.Add(new ScatterSeries()
            {
                Values = new ChartValues<int>()
            });

            

            var allHistory = datastore.LoadExerciseHistory(exercise);

            foreach (var his in allHistory)
            {
                GraphData[0].Values.Add(his.TimeCompleted.DayOfYear);
                
            }
        }

        private void ShowAllExerciseCounts() 
        {
            GraphData = new SeriesCollection();

            GraphData.Add(new ColumnSeries());
            GraphData[0].Values = new ChartValues<int>();

            Seperator = new Separator(){IsEnabled = false};


            XLabels = new List<string>();


            foreach (var exerciseItem in AllExerciseItems)
            {
                List<ExerciseHistoryItem> history = datastore.LoadExerciseHistory(exerciseItem);

                GraphData[0].Values.Add(history.Count);
                XLabels.Add(exerciseItem.ExerciseName);
            }
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
    }
}
