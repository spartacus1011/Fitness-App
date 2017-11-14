using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using Exercise_tracker.Helpers;
using Exercise_tracker.ViewModels;
using Exercise_tracker.Classes;
using Exercise_tracker.Views;
using MvvmDialogs;


namespace Exercise_tracker.ViewModels
{
    class MainWindowViewModel:ObservableObject
    {
        public static IDialogService dialogService;
        public List<ExerciseItem> AllExerciseItems = new List<ExerciseItem>();
        public ObservableCollection<ExerciseItem> ExerciseItemsToDo { get; set;}

        public ICommand ShowCreateExerciseCommand { get { return new DelegateCommand(ShowCreateExerciseDialog); } }
        public ICommand ShowEditRosterCommand { get { return new DelegateCommand(ShowEditRosterDialog); } }

        private readonly string rootProgramDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string allExercisesXmlFilename = "AllExerciseItemsList.xml";

        private readonly DispatcherTimer updateTimer = new DispatcherTimer();

        public MainWindowViewModel()
        {
            dialogService = new DialogService();
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            ExerciseItemsToDo = new ObservableCollection<ExerciseItem>();

            updateTimer.Tick += OnUpdateTimerTick;
            updateTimer.Interval = new TimeSpan(0, 1, 0); //every 1 minute
            updateTimer.Start();

            LoadExerciseList();

            //DB testing--------------------------------------------
            //DatabaseHelper.CreateTheStrings();
            string dbPath = rootProgramDirectory + "AllTheExercise.db";

            try
            {
                if (File.Exists(dbPath))
                    File.Delete(dbPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            SQLiteConnection connection = DatabaseHelper.ConnectToDatabase(dbPath);
            DatabaseHelper.CreateDatabase(dbPath); //pretty sure these will have the same outcome if the db was already created

            DatabaseHelper.CreateNewExerciseTable(connection);

        }

        private void RebuildList()
        {
            List<ExerciseItem> temp = ExerciseItemsToDo.OrderBy(x => x.DueTime).ToList();

            ExerciseItemsToDo.Clear();
            foreach (var item in temp)
            {
                ExerciseItemsToDo.Add(item);
            }
        }

        private void SaveExerciseList() //this destroys the file and starts again. not the best way. good enough for now
        {
            XmlHelper.ToXmlFile(AllExerciseItems, rootProgramDirectory + allExercisesXmlFilename);
        }

        private void LoadExerciseList()
        {
            try
            {
                AllExerciseItems = XmlHelper.FromXmlFile<List<ExerciseItem>>(rootProgramDirectory + allExercisesXmlFilename);

                foreach (ExerciseItem item in AllExerciseItems)
                {
                    item.MarkExerciseCompletedChanged += OnMarkExerciseCompletedChanged; //attach all the handlers regardless. it should make things a bit easier later on
                    item.DeleteExercise += OnDeleteExercise;
                    item.EditExericse += OnEditExercise;
                    if (item.IsUsedInRoster)
                        ExerciseItemsToDo.Add(item);
                }
                OnUpdateTimerTick(null, null); //first time fire to update everything on load
            }
            catch (Exception ex)
            {
                if (ex is System.IO.DirectoryNotFoundException || ex is System.IO.FileNotFoundException)
                    Debug.WriteLine("No Exercise Item List found in: " + rootProgramDirectory);
                else
                    throw;
            }
        }


        #region CommandFunctions
        private void ShowCreateExerciseDialog()
        {
            var dialogViewModel = new CreateExerciseViewModel();

            bool? success = dialogService.ShowDialog<CreateExerciseView>(this, dialogViewModel);

            DateTime today = DateTime.Today;

            if (success == true)
            {
                if(dialogViewModel.ItemToAdd.IsDailyRecurrence)
                    dialogViewModel.ItemToAdd.DueTime = today;
                else if (dialogViewModel.ItemToAdd.IsWeeklyRecurrence)
                {
                    // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
                    int requiredDueDay = ((int)dialogViewModel.ItemToAdd.WeeklyRecurrenceDay - (int)today.DayOfWeek + 7) % 7;
                    dialogViewModel.ItemToAdd.DueTime = today.AddDays(requiredDueDay);
                }
                else if (dialogViewModel.ItemToAdd.IsMonthlyRecurrence)
                {
                    int requiredDueMonth = ((int) dialogViewModel.ItemToAdd.MonthlyRecurrenceDay - (int)today.Month + DateTime.DaysInMonth(today.Year,today.Month)) % DateTime.DaysInMonth(today.Year,today.Month) ;
                    dialogViewModel.ItemToAdd.DueTime = today.AddDays(requiredDueMonth + 1); //dont ask me why i need a plus 1. I dont know
                }

                AllExerciseItems.Add(dialogViewModel.ItemToAdd);
                SaveExerciseList();

                if (dialogViewModel.ItemToAdd.IsUsedInRoster)
                {
                    dialogViewModel.ItemToAdd.MarkExerciseCompletedChanged += OnMarkExerciseCompletedChanged; //check that you dont need to explicitly unsub when this item is removed
                    dialogViewModel.ItemToAdd.EditExericse += OnEditExercise;
                    dialogViewModel.ItemToAdd.DeleteExercise += OnDeleteExercise;
                    //dialogViewModel.ItemToAdd.RequiredSetsCount = dialogViewModel.ItemToAdd.RequiredSets; //set the sets. dont really need an if statement here doesnt matter if 0=0
                    //the above line seems kinda odd and not necessary. Check to see if it breaks anything
                    ExerciseItemsToDo.Add(dialogViewModel.ItemToAdd);
                    OnUpdateTimerTick(null, null);
                }
            }
            RebuildList();
        }
        
        private void ShowEditRosterDialog()
        {
            var dialogViewModel = new EditRosterViewModel(AllExerciseItems);
            List<ExerciseItem> temp = AllExerciseItems.ToList();

            bool? success = dialogService.ShowDialog<EditRosterView>(this, dialogViewModel);
           
            if (success == true)
            {
                ExerciseItemsToDo.Clear();
                foreach (var rosterItem in dialogViewModel.RosterItems)
                {
                    ExerciseItem exerciseItem = temp.FirstOrDefault(x => x.GUIDID == rosterItem.GUIDID);
                    exerciseItem.IsUsedInRoster = rosterItem.IsUsedInRoster;
                    RaisePropertyChangedEvent("IsUsedInRoster");
                    if (!temp.Contains(exerciseItem))
                    {
                        exerciseItem.DueTime = DateTime.Now;
                    }
                    if(exerciseItem.IsUsedInRoster)
                        ExerciseItemsToDo.Add(exerciseItem);
                }

                RebuildList();
            }
            else //revert it to the way it was
            {
                //foreach (ExerciseItem item in temp)
                //{
                //    ExerciseItemsToDo.Add(item);
                //}

                //RebuildList();
            }
        }
        #endregion


        #region Events
        private void OnProcessExit(object sender, EventArgs e)
        {
            SaveExerciseList();
        }

        void OnMarkExerciseCompletedChanged(object sender, EventArgs e)
        {
            RebuildList();
        }

        void OnDeleteExercise(object sender, EventArgs e)
        {
            ExerciseItem itemToDelete = sender as ExerciseItem;
            ExerciseItemsToDo.Remove(itemToDelete);
            AllExerciseItems.Remove(itemToDelete);
            RebuildList();
        }

        void OnEditExercise(object sender, EventArgs e)
        {
            ExerciseItem itemToEdit = sender as ExerciseItem;

            //This is a somewhat custom version of the create exercise window
            var dialogViewModel = new CreateExerciseViewModel();
            dialogViewModel.ItemToAdd = itemToEdit;
            bool? success = dialogService.ShowDialog<CreateExerciseView>(this, dialogViewModel);

            if (success == true)
            {
                SaveExerciseList(); //might be a bad idea saving here. only time will tell

                if (dialogViewModel.ItemToAdd.IsUsedInRoster)
                {
                    OnUpdateTimerTick(null, null);
                }
            }
        }

        private void OnUpdateTimerTick(object sender, EventArgs e) //update the due time for every item
        {
            foreach (var item in ExerciseItemsToDo)
            {
                item.UpdateTime();
            }
            RebuildList();
        }
        #endregion
    }
}
