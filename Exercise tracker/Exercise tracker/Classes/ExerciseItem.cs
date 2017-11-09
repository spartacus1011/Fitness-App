using System;
using Exercise_tracker.Helpers;
using Exercise_tracker.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Exercise_tracker.Classes
{
    public class ExerciseItem: ObservableObject
    {
        public event EventHandler MarkExerciseCompletedChanged;
        public event EventHandler DeleteExercise;
        public event EventHandler EditExericse;
        public ICommand MarkExerciseCompletedCommand { get { return new DelegateCommand(MarkExerciseCompleted); } }
        
        public ICommand DeleteThisExerciseCommand { get { return new DelegateCommand(DeleteThisExerciseItem); } }
        public ICommand EditThisExerciseCommand { get { return new DelegateCommand(EditThisExerciseItem); } }

        public string GUIDID { get; set; } //This id will mainly be used to link the exercise item back to its history component counterpart
        public string ExerciseName { get; set; }
        public ExercisetypeEnum Exercisetype { get; set; }
        public bool IsRepetitions { get { return Exercisetype == ExercisetypeEnum.SingleReps || Exercisetype == ExercisetypeEnum.NormalSets; } } //might be worth investing in an enum to bool converter for wpf
        public bool IsTimed { get { return Exercisetype == ExercisetypeEnum.Timed || Exercisetype == ExercisetypeEnum.TimedSets; } }
        public bool IsSets { get { return Exercisetype == ExercisetypeEnum.NormalSets || Exercisetype == ExercisetypeEnum.TimedSets; } }
        public int RequiredReps { get; set; }
        public int RequiredTime { get; set; } //make this a timespan
        public int RequiredSets { get; set; }
        public int RequiredSetsCount { get; set; }
        public ExerciseTimeUnitsEnum ExerciseTimeUnits { get; set; }
        public bool IsUsingRestTime { get; set; }
        public ExerciseRestTimeEnum RestTime { get; set; } //make this a timespan 
        public bool IsUsedInRoster { get; set; }
        public int TotalCompletedCount { get; set; } //this is the total amount of times this exercise has been done (To be replaced by a proper history thing)
        public bool IsOneTimeExercise { get; set; } //use this to signify that it was a one time exercise. these guys probably wont be shown in the to do list rather a history list. Until history list is done, this will just add 1 to the total count
        public DateTime DueTime { get; set; }
        public bool OnTime { get { return DueTime.Day == DateTime.Now.Day; } }
        public bool Late { get { return DueTime.Day < DateTime.Now.Day; } }
        public bool Early { get { return DueTime.Day > DateTime.Now.Day; } }
        public string DueTimeAsString
        {
            get
            {
                if (Late)
                    return "Due " + (DateTime.Now.Day - DueTime.Day) + " days ago";
                if (OnTime)
                    return "Due Today";
                if (Early)
                    return "Due in " + Math.Abs(DateTime.Now.Day - DueTime.Day) + " days";
                return ""; //should be impossible to get to this point
            }
        }
        public string ShownCount //maybe look at changing this and making it a datatrigger thing if possible
        {
            get
            {
                switch (Exercisetype)
                {
                    case ExercisetypeEnum.Timed:
                    case ExercisetypeEnum.TimedSets:
                        switch (ExerciseTimeUnits)
                        {
                            case ExerciseTimeUnitsEnum.Hours:
                                return RequiredTime + " Hours";
                            case ExerciseTimeUnitsEnum.Minutes:
                                return RequiredTime + " Minutes";
                            case ExerciseTimeUnitsEnum.Seconds:
                                return RequiredTime + " Seconds";
                        }
                        break;
                    case ExercisetypeEnum.NormalSets:
                    case ExercisetypeEnum.SingleReps:
                        return RequiredReps.ToString();
                }
                return "";
            }
        }
        public ExerciseRecurrenceEnum ExerciseRecurrence { get; set; }
        public TimeSpan RecurranceTimeSpan
        {
            get
            {
                switch (ExerciseRecurrence)
                {
                    case ExerciseRecurrenceEnum.Daily:
                        return new TimeSpan(1, 0, 0, 0);
                    case ExerciseRecurrenceEnum.Weekly:
                        return new TimeSpan(7, 0, 0, 0);
                    case ExerciseRecurrenceEnum.Monthly:
                        return new TimeSpan(30, 0, 0, 0); //this obiously wont work properly find a more suitable way around this
                }
                return new TimeSpan(0);
            }
        }
        //End Variables

        public ExerciseItem() //this is the saving a temp using one
        {
        }

        public ExerciseItem(string GUIDID) //this is the proper creation one
        {
            this.GUIDID = GUIDID;
            
        }

        private void MarkExerciseCompleted()
        {
            //Are you sure?
            MessageBoxResult result = MainWindowViewModel.dialogService.ShowMessageBox(
                this,
                "Are you sure you want to complete this exercise?",
                "Complete Exercise",
                MessageBoxButton.YesNo
            );

            if (result != MessageBoxResult.Yes)
                return;

            if (IsSets)
            {
                RequiredSetsCount--;
                if (RequiredSetsCount <= 0)
                {
                    FullComplete();
                }
            }
            else
                FullComplete();
            UpdateViews();
        }



        private void DeleteThisExerciseItem()
        {
            //are you sure?
            MessageBoxResult result = MainWindowViewModel.dialogService.ShowMessageBox(
                this,
                "Are you sure you want to delete this exercise?",
                "Delete Exericse",
                MessageBoxButton.YesNo
            );

            if (result != MessageBoxResult.Yes)
                return;

            DeleteExercise?.Invoke(this, null);
        }

        private void EditThisExerciseItem()
        {
            EditExericse?.Invoke(this, null);
        }

        public void UpdateTime() //this guy will get called once on startup and once every 1 minutes
        {
            UpdateViews();
        }

        private void FullComplete()
        {
            RequiredSetsCount = RequiredSets;
            DueTime = DueTime.Add(RecurranceTimeSpan);
            TotalCompletedCount += 1;

            MarkExerciseCompletedChanged?.Invoke(this, null);
        }

        private void UpdateViews()
        {
            RaisePropertyChangedEvent("ExerciseName");
            RaisePropertyChangedEvent("RequiredSetsCount");
            RaisePropertyChangedEvent("DueTimeAsString");
            RaisePropertyChangedEvent("OnTime");
            RaisePropertyChangedEvent("Early");
            RaisePropertyChangedEvent("Late");
        }
    }
}