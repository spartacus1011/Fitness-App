using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;
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
        public ObservableCollection<ExerciseItem> ExerciseItemsToDo { get { return exerciseItemsToDo; } }

        private readonly string rootProgramDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string allExercisesXmlFilename = "AllExerciseItemsList.xml";
        private readonly ObservableCollection<ExerciseItem> exerciseItemsToDo = new ObservableCollection<ExerciseItem>(); //this works. but i dont think that it is the right way to do things
        private readonly DispatcherTimer updateTimer = new DispatcherTimer();

        public MainWindowViewModel()
        {
            dialogService = new DialogService();
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            updateTimer.Tick += OnUpdateTimerTick;
            updateTimer.Interval = new TimeSpan(0, 1, 0); //every 1 minute
            updateTimer.Start();

            LoadExerciseList();
        }

        private void RebuildList()
        {
            List<ExerciseItem> temp = exerciseItemsToDo.OrderBy(x => x.DueTime).ToList();
            exerciseItemsToDo.Clear();
            foreach (var item in temp)
            {
                exerciseItemsToDo.Add(item);
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
                        exerciseItemsToDo.Add(item);
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


        #region Commands
        public ICommand ShowCreateExerciseCommand { get { return new DelegateCommand(ShowCreateExerciseDialog); } }
        private void ShowCreateExerciseDialog()
        {
            var dialogViewModel = new CreateExerciseViewModel();

            bool? success = dialogService.ShowDialog<CreateExerciseView>(this, dialogViewModel);

            if (success == true)
            {
                AllExerciseItems.Add(dialogViewModel.ItemToAdd);
                SaveExerciseList();

                if (dialogViewModel.ItemToAdd.IsUsedInRoster)
                {
                    dialogViewModel.ItemToAdd.MarkExerciseCompletedChanged += OnMarkExerciseCompletedChanged; //check that you dont need to explicitly unsub when this item is removed
                    dialogViewModel.ItemToAdd.EditExericse += OnEditExercise;
                    dialogViewModel.ItemToAdd.DeleteExercise += OnDeleteExercise;
                    dialogViewModel.ItemToAdd.RequiredSetsCount = dialogViewModel.ItemToAdd.RequiredSets; //set the sets. dont really need an if statement here doesnt matter if 0=0
                    exerciseItemsToDo.Add(dialogViewModel.ItemToAdd);
                    OnUpdateTimerTick(null, null);
                }
            }
        }
        public ICommand ShowAddExerciseCommand { get { return new DelegateCommand(ShowAddExerciseDialog); } }
        private void ShowAddExerciseDialog()
        {
            var dialogViewModel = new EditRosterViewModel(AllExerciseItems);

            bool? success = dialogService.ShowDialog<EditRosterView>(this, dialogViewModel);

            List<ExerciseItem> temp = exerciseItemsToDo.ToList();

            ExerciseItemsToDo.Clear(); //recreate list regardless
            foreach (var item in dialogViewModel.AllExerciseItemsToAdd.Where(x => x.IsUsedInRoster))
            {
                if (!temp.Contains(item))
                {
                    item.DueTime = DateTime.Now;
                }
                ExerciseItemsToDo.Add(item);
            }

            RebuildList();
            if (success == true) //Not really actually using success
            {

            }
        }

        #endregion


        #region Events
        private void OnProcessExit(object sender, EventArgs e)
        {
            SaveExerciseList();
        }

        void OnMarkExerciseCompletedChanged(object sender, EventArgs e) //these events have to be public for now for the test enviroment so that they can get properly subscribed to when we create the exercise
        {
            RebuildList();
            //display short message of yes or no confirmation
        }

        void OnDeleteExercise(object sender, EventArgs e)
        {
            ExerciseItem itemToDelete = sender as ExerciseItem;
            exerciseItemsToDo.Remove(itemToDelete);
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
