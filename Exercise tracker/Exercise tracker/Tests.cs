using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Exercise_tracker.Classes;
using Exercise_tracker.ViewModels;
using Exercise_tracker.Views;
using MvvmDialogs;
using Moq;
using NUnit.Framework;

namespace Exercise_tracker
{
    //This is just temporary until you can be bothered fixing this all up properly
    [TestFixture]
    public class Tests
    {
        private MainWindowViewModel mainWindowViewModel;
        private Mock<IDialogService> dialogService;
        private const string SKIP_SETUP = "SkipSetup";
        string rootProgramDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd("Debug".ToCharArray()); //just make sure to change these locations when the real ones change
        string allExercisesXmlFilename = "DebugAllExerciseItemsList.xml";
        private string oldXmlFile = "oldFile.xml";

        [SetUp]
        public void SetUp()
        {
            if (!CheckForSkipSetup())
            {
                string path = rootProgramDirectory + allExercisesXmlFilename;
                string oldPath = rootProgramDirectory + oldXmlFile;


                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
                if (File.Exists(path)) //keep at least one version of the older xml just incase it is needed for anything
                    File.Move(path, oldPath);
                else
                {
                    //Console.WriteLine("File Not Found. Ensure test file location is correct. \n File Location: " + path); //not using at the moment
                }
                CreateThings();
            }
        }

        private void CreateThings()
        {
            dialogService = new Mock<IDialogService>();
            mainWindowViewModel = new MainWindowViewModel();
            MainWindowViewModel.dialogService = dialogService.Object; //very cheaty, but it works and works well
            clearExercises();
        }

        private static bool CheckForSkipSetup()
        {
            ArrayList categories = TestContext.CurrentContext.Test
                .Properties["_CATEGORIES"] as ArrayList;

            bool skipSetup = categories != null && categories.Contains(SKIP_SETUP);
            return skipSetup;
        }

        [Test]
        public void CreateExerciseTest() //probalby covered almost everywhereelse
        {
            string testExerciseName = "I am a test";

            dialogService
                .Setup(mock => mock.ShowDialog<CreateExerciseView>(mainWindowViewModel, It.IsAny<CreateExerciseViewModel>()))
                .Returns(true)
                .Callback((INotifyPropertyChanged ownerViewModel, CreateExerciseViewModel addExerciseViewModel) =>
                    addExerciseViewModel.ExerciseNameToAdd = testExerciseName);

            mainWindowViewModel.ShowCreateExerciseCommand.Execute(null);

            Assert.IsTrue(mainWindowViewModel.ExerciseItemsToDo.Any(x => x.ExerciseName == testExerciseName));
        }

        [Test, Timeout(10000)] //I guess this is ok for it to create 1000 exercises in under 10 seconds. Watch this though
        [Ignore("Takes to long. Do check every so often")]
        public void StressTest1()
        {
            string ExerciseBaseName = "Exercise: ";
            Stopwatch watch = new Stopwatch();
            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            for (int i = 0; i < 1000; i++)
            {
                watch.Start();
                dialogService
                    .Setup(mock => mock.ShowDialog<CreateExerciseView>(mainWindowViewModel, It.IsAny<CreateExerciseViewModel>()))
                    .Returns(true)
                    .Callback((INotifyPropertyChanged ownerViewModel, CreateExerciseViewModel addExerciseViewModel) =>
                        addExerciseViewModel.ExerciseNameToAdd = ExerciseBaseName + i);

                mainWindowViewModel.ShowCreateExerciseCommand.Execute(null);
                watch.Stop();
                //Console.WriteLine(i + " : " + watch.Elapsed);
                watch.Reset();
                Assert.IsTrue(mainWindowViewModel.ExerciseItemsToDo.Any(x => x.ExerciseName == ExerciseBaseName + i));
            }
            watch2.Stop();
            Console.WriteLine("Total time: " + watch2.Elapsed);

        }

        [Test]
        public void AddToRosterTest()
        {
            string testExerciseName = "Test";

            showAndCreateExercise(testExerciseName, false);

            Assert.False(mainWindowViewModel.ExerciseItemsToDo.Any(x => x.ExerciseName == testExerciseName));

            List<ExerciseItem> exercisesToAdd = mainWindowViewModel.AllExerciseItems.Where(x => x.ExerciseName == testExerciseName).ToList();
            Assert.AreEqual(1, exercisesToAdd.Count); //commented out for now. this will fail if the test is run more than once without clearing the list. Add back in once ClearSaved is implemented

            showAndAddExercise(exercisesToAdd);

            Assert.True(mainWindowViewModel.ExerciseItemsToDo.Any(x => x.ExerciseName == testExerciseName));
        }

        [Test]
        public void EnsureMainListStaysInCorrectOrder()
        {
            List<ExerciseItem> TestList = new List<ExerciseItem>();
            TestList.Add(new ExerciseItem() { ExerciseName = "2", DueTime = DateTime.Now - new TimeSpan(1, 0, 0, 0) }); //yesterday
            TestList.Add(new ExerciseItem() { ExerciseName = "4", DueTime = DateTime.Now + new TimeSpan(1, 0, 0, 0) }); //tomorrow
            TestList.Add(new ExerciseItem() { ExerciseName = "0", DueTime = DateTime.Now - new TimeSpan(7, 0, 0, 0) }); //last week 
            TestList.Add(new ExerciseItem() { ExerciseName = "5", DueTime = DateTime.Now + new TimeSpan(7, 0, 0, 0) }); //next week
            TestList.Add(new ExerciseItem() { ExerciseName = "3", DueTime = DateTime.Now }); //today

            clearExercises();

            foreach (var item in TestList)
            {
                mainWindowViewModel.ExerciseItemsToDo.Add(item);
            }

            showAndCreateExercise("1", dueTime: DateTime.Now - new TimeSpan(2, 0, 0, 0)); //2 days ago //This should cause the list to re order

            for (int i = 0; i <= TestList.Count; i++)
            {
                Assert.AreEqual(mainWindowViewModel.ExerciseItemsToDo.ElementAt(i).ExerciseName, i.ToString());
            }

        }

        [Test]
        public void CanCompleteExercises()
        {
            string exerciseName = "Test";

            showAndCreateExercise(exerciseName, dueTime: DateTime.Now);

            ExerciseItem theOne = showAndCreateExercise(exerciseName, dueTime: DateTime.Now);

            CompleteExercise(theOne);

            Assert.AreEqual(theOne.DueTime.Day, (DateTime.Now + new TimeSpan(1, 0, 0, 0)).Day);
        }

        [Test]
        [NUnit.Framework.Category(SKIP_SETUP)]
        public void LoadAndSaveTest()
        {

            string path = rootProgramDirectory + allExercisesXmlFilename;

            if (File.Exists(path))
                File.Delete(path);
            else
            {
                //Console.WriteLine("File Not Found. Ensure test file location is correct. \n File Location: " + path); //not using at the moment
            }
            CreateThings();

            showAndCreateExercise("Test1");

            Assert.AreEqual(1, mainWindowViewModel.ExerciseItemsToDo.Count);
            Assert.AreEqual("Test1", mainWindowViewModel.ExerciseItemsToDo.ElementAt(0).ExerciseName);

            if (File.Exists(path))
                File.Delete(path);
            else
            {
                Assert.Fail("There needs to be a saved file at this point Ensure test file location is correct. \n File Location: " + path);
            }

            mainWindowViewModel = null;
            mainWindowViewModel = new MainWindowViewModel();

            Assert.AreEqual(0, mainWindowViewModel.ExerciseItemsToDo.Count);
        }

        [Test]
        public void CanDeleteExercise()
        {
            string ExerciseName = "Test";

            ExerciseItem theOne = showAndCreateExercise(ExerciseName);

            DeleteExercise(theOne);

            dialogService.VerifyAll();
            Assert.IsFalse(mainWindowViewModel.ExerciseItemsToDo.Any(x => x.ExerciseName == ExerciseName));

        }

        //Jig/HelperFunctions----------------------------------------------------------------------------------------------
        //If this gets to big and messy, could probably move it to its own helper class provided that doesnt make more problems

        private ExerciseItem showAndCreateExercise(string exerciseName, bool isUsedInRoster = true, DateTime dueTime = new DateTime())
        {
            ExerciseItem addedExercise = new ExerciseItem();
            dialogService
                .Setup(mock => mock.ShowDialog<CreateExerciseView>(mainWindowViewModel, It.IsAny<CreateExerciseViewModel>()))
                .Returns(true)
                .Callback((INotifyPropertyChanged ownerViewModel, CreateExerciseViewModel createExerciseViewModel) =>
                {
                    createExerciseViewModel.ExerciseNameToAdd = exerciseName;
                    createExerciseViewModel.IsUsedInRoster = isUsedInRoster;
                    createExerciseViewModel.ItemToAdd.DueTime = dueTime;
                    addedExercise = createExerciseViewModel.ItemToAdd;
                });
            mainWindowViewModel.ShowCreateExerciseCommand.Execute(null);
            return addedExercise;
        }

        private void showAndAddExercise(List<ExerciseItem> itemsToAdd = null, List<ExerciseItem> itemsToRemove = null)
        {
            if (itemsToAdd == null)
                itemsToAdd = new List<ExerciseItem>();

            if (itemsToRemove == null)
                itemsToRemove = new List<ExerciseItem>();

            if (itemsToAdd.Any(x => x.IsUsedInRoster))
                throw new Exception("Attempting to Add item that is already in roster");
            if (itemsToRemove.Any(x => !x.IsUsedInRoster))
                throw new Exception("Attempting to Remove item that isnt already in roster");

            dialogService
                .Setup(mock => mock.ShowDialog<EditRosterView>(mainWindowViewModel, It.IsAny<EditRosterViewModel>()))
                .Returns(true)
                .Callback((INotifyPropertyChanged ownerViewModel, EditRosterViewModel addExerciseViewModel) =>
                {
                    foreach (var item in itemsToAdd)
                    {
                        item.ToggleAddToRosterCommand.Execute(null);
                    }

                    foreach (var item in itemsToRemove)
                    {
                        item.ToggleAddToRosterCommand.Execute(null);
                    }
                }
                   );

            mainWindowViewModel.ShowEditRosterCommand.Execute(null);
        }

        private void CompleteExercise(ExerciseItem exerciseItem)
        {
            dialogService.Setup(mock => //must 100% match the one used in code. Should make these a single item so that it can be called easier
                    mock.ShowMessageBox(
                        exerciseItem,
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        MessageBoxButton.YesNo,
                        MessageBoxImage.None,
                        MessageBoxResult.None))
                .Returns(MessageBoxResult.Yes);

            exerciseItem.MarkExerciseCompletedCommand.Execute(null);
        }

        private void DeleteExercise(ExerciseItem exerciseItem)
        {
            dialogService.Setup(mock => //must 100% match the one used in code. Should make these a single item so that it can be called easier
                    mock.ShowMessageBox(
                        exerciseItem,
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        MessageBoxButton.YesNo,
                        MessageBoxImage.None,
                        MessageBoxResult.None))
                .Returns(MessageBoxResult.Yes);

            exerciseItem.DeleteThisExerciseCommand.Execute(null);
        }

        private void clearExercises()
        {
            //need to implement a real method to delete them first!
            mainWindowViewModel.ExerciseItemsToDo.Clear();
        }

    }
}
