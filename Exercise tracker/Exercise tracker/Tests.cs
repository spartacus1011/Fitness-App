using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Exercise_tracker.Classes;
using Exercise_tracker.Helpers;
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
        //string rootProgramDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd("Debug".ToCharArray()); //just make sure to change these locations when the real ones change
        //private string dbname = "DebugAllTheExercise.db";
        private string dbPath;

        [SetUp]
        public void SetUp()
        {
            if (!CheckForSkipSetup())
            {
                CreateThings();
            }
        }

        private void CreateThings()
        {
            dialogService = new Mock<IDialogService>();
            mainWindowViewModel = new MainWindowViewModel(dialogService);
        }

        private static bool CheckForSkipSetup()
        {
            ArrayList categories = TestContext.CurrentContext.Test
                .Properties["_CATEGORIES"] as ArrayList;

            bool skipSetup = categories != null && categories.Contains(SKIP_SETUP);
            return skipSetup;
        }

        [TearDown]
        public void tearDown()
        {

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

        [Test, Timeout(3000)] 
        //[Ignore("Takes to long. Do check every so often")] //DB is faster!!!
        //Test is indicative of change ONLY!!! not real read/write times due to test DB being stored in memory
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
        [Ignore("Fails because i havent fixed the db yet")]
        public void EnsureMainListGetsLoadedCorrectly() //should put a whole lot of other db tests here
        {
            string tempdbPath = CreateTempDB("CorrectLoadOrder");

            mainWindowViewModel = new MainWindowViewModel(dialogService, true, tempdbPath); //just override the old one

            showAndCreateExercise("odd one");
            mainWindowViewModel.ExerciseItemsToDo.FirstOrDefault(x => x.ExerciseName == "odd one").DueTime = DateTime.Now + new TimeSpan(365,0,0,0);
            CompleteExercise(mainWindowViewModel.ExerciseItemsToDo.FirstOrDefault(x => x.ExerciseName == "odd one"));
            
            Console.WriteLine("Exercise order:");
            foreach (var ex in mainWindowViewModel.ExerciseItemsToDo)
            {
                Console.WriteLine(ex.ExerciseName);
            }

            for (int i = 0; i < mainWindowViewModel.ExerciseItemsToDo.Count -1; i++) //the -1 is to exclude "odd one"
            {
                Assert.AreEqual(i.ToString(), mainWindowViewModel.ExerciseItemsToDo.ElementAt(i).ExerciseName);
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
        [Ignore("Need new tests to verify the database!!")]
        public void LoadAndSaveTestXML()
        {
            showAndCreateExercise("Test1");

            Assert.AreEqual(1, mainWindowViewModel.ExerciseItemsToDo.Count);
            Assert.AreEqual("Test1", mainWindowViewModel.ExerciseItemsToDo.ElementAt(0).ExerciseName);

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
                .Callback((INotifyPropertyChanged ownerViewModel, EditRosterViewModel editExerciseViewModel) =>
                {
                    foreach (var item in itemsToAdd)
                    {
                        //the possible null exception is good as it means something has gone wrong 
                        editExerciseViewModel.RosterItems.FirstOrDefault(x => x.GUIDID == item.GUIDID).ToggleAddToRosterCommand.Execute(null);
                    }

                    foreach (var item in itemsToRemove)
                    {
                        //the possible null exception is good as it means something has gone wrong 
                        editExerciseViewModel.RosterItems.FirstOrDefault(x => x.GUIDID == item.GUIDID).ToggleAddToRosterCommand.Execute(null);
                    }
                });

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

        private string CreateTempDB(string fileName)
        {
            string dbPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));
            dbPath += "\\TestDatabases\\" + fileName + ".db";
            dbPath = dbPath.TrimStart(("file:\\").ToCharArray());
            string dbCopyPath = dbPath + "Copy.db";

            if(File.Exists(dbCopyPath))
                File.Delete(dbCopyPath);

            File.Copy(dbPath, dbCopyPath);

            return dbCopyPath;
        }

    }
}
