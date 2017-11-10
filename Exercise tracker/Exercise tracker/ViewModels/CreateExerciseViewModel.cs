using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MvvmDialogs;
using Exercise_tracker.Helpers;
using Exercise_tracker.Classes;

namespace Exercise_tracker.ViewModels
{
    public class CreateExerciseViewModel : ObservableObject, IModalDialogViewModel
    {
        public ExerciseItem ItemToAdd
        {
            get { return itemToAdd; }
            set //for when we use edit mode
            {
                itemToAdd = value;
                RaiseAllTheEvents();
            }
        }
        private ExerciseItem itemToAdd;
        private bool? dialogResult;
        public ICommand CloseDialogTrueCommand { get { return new DelegateCommand(CloseDialogTrue); } }
        public string ExerciseNameToAdd { get { return itemToAdd.ExerciseName; } set { itemToAdd.ExerciseName = value; } }
        public bool IsRepetitions { get { return itemToAdd.IsRepetitions; } }
        public bool IsTimed { get { return itemToAdd.IsTimed; } }
        public bool IsSets { get { return itemToAdd.IsSets; } }
        public int Repetitions
        {
            get { return itemToAdd.RequiredReps; }
            set
            {
                itemToAdd.RequiredReps = value;
                RaisePropertyChangedEvent("Repetitions");
            }
        }
        public int ExerciseTime
        {
            get { return itemToAdd.RequiredTime; }
            set
            {
                itemToAdd.RequiredTime = value;
                RaisePropertyChangedEvent("ExerciseTime");
            }
        }
        public int SetsCount
        {
            get { return itemToAdd.RequiredSets; }
            set
            {
                itemToAdd.RequiredSets = value;
                RaisePropertyChangedEvent("SetsCount");
            }
        }
        public bool IsUsingRestTime
        {
            get { return itemToAdd.IsUsingRestTime; }
            set
            {
                itemToAdd.IsUsingRestTime = value;
                RaisePropertyChangedEvent("IsUsingRestTime");
            }
        }
        public IEnumerable<ExerciseRestTimeEnum> ExerciseRestTimeValues { get { return Enum.GetValues(typeof(ExerciseRestTimeEnum)).Cast<ExerciseRestTimeEnum>(); } }
        public ExerciseRestTimeEnum RestTime
        {
            get { return itemToAdd.RestTime; }
            set
            {
                itemToAdd.RestTime = value;
                RaisePropertyChangedEvent("RestTime");
            }
        }
        public IEnumerable<ExercisetypeEnum> ExerciseTypeValues { get { return Enum.GetValues(typeof(ExercisetypeEnum)).Cast<ExercisetypeEnum>(); } }
        public ExercisetypeEnum SelectedExerciseType
        {
            get { return itemToAdd.Exercisetype; }
            set
            {
                itemToAdd.Exercisetype = value;
                RaisePropertyChangedEvent("SelectedExerciseType");
                RaisePropertyChangedEvent("IsRepetitions");
                RaisePropertyChangedEvent("IsTimed");
                RaisePropertyChangedEvent("IsSets");
            }
        }
        public IEnumerable<ExerciseRecurrenceEnum> ExerciseRecurrenceValues { get { return Enum.GetValues(typeof(ExerciseRecurrenceEnum)).Cast<ExerciseRecurrenceEnum>(); } }
        public ExerciseRecurrenceEnum SelectedExerciseRecurrence
        {
            get { return itemToAdd.ExerciseRecurrence; }
            set
            {
                itemToAdd.ExerciseRecurrence = value;
                RaisePropertyChangedEvent("SelectedExerciseRecurrence");
                RaisePropertyChangedEvent("IsWeekly");
                RaisePropertyChangedEvent("IsMonthly");
            }
        }
        public IEnumerable<ExerciseTimeUnitsEnum> ExerciseTimeUnitValues { get { return Enum.GetValues(typeof(ExerciseTimeUnitsEnum)).Cast<ExerciseTimeUnitsEnum>(); } }
        public ExerciseTimeUnitsEnum SelectedExerciseTimeUnits
        {
            get { return itemToAdd.ExerciseTimeUnits; }
            set
            {
                itemToAdd.ExerciseTimeUnits = value;
                RaisePropertyChangedEvent("SelectedExerciseTimeUnits");
            }
        }
        public bool IsWeekly { get { return itemToAdd.IsWeeklyRecurrence; } }
        public IEnumerable<DayOfWeek> DaysOfTheWeekValues { get { return Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>(); } }
        public DayOfWeek SelectedDayOfTheWeek
        {
            get { return itemToAdd.WeeklyRecurrenceDay; }
            set
            {
                itemToAdd.WeeklyRecurrenceDay = value;
                RaisePropertyChangedEvent("SelectedDayOfTheWeek");
            }
        }

        public bool IsMonthly { get { return itemToAdd.IsMonthlyRecurrence; } }
        public IEnumerable<int> DaysInThisMonth
        {
            get { return Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)); }
        }

        public int SelectedDayOfTheMonth
        {
            get { return itemToAdd.MonthlyRecurrenceDay; }
            set
            {
                itemToAdd.MonthlyRecurrenceDay = value;
                RaisePropertyChangedEvent("SelectedDayOfTheMonth");
            }
        }

        public bool IsUsedInRoster
        {
            get { return itemToAdd.IsUsedInRoster; }
            set
            {
                itemToAdd.IsUsedInRoster = value;
                RaisePropertyChangedEvent("IsUsedInRoster");
            }
        }

        public CreateExerciseViewModel()
        {
            Guid newGUIDID = Guid.NewGuid();
            itemToAdd = new ExerciseItem(newGUIDID.ToString());
            IsUsedInRoster = true;
        }

        private void RaiseAllTheEvents()
        {
            RaisePropertyChangedEvent("Repetitions");
            RaisePropertyChangedEvent("ExerciseTime");
            RaisePropertyChangedEvent("SetsCount");
            RaisePropertyChangedEvent("IsUsingRestTime");
            RaisePropertyChangedEvent("RestTime");
            RaisePropertyChangedEvent("SelectedExerciseType");
            RaisePropertyChangedEvent("IsRepetitions");
            RaisePropertyChangedEvent("IsTimed");
            RaisePropertyChangedEvent("IsSets");
            RaisePropertyChangedEvent("SelectedExerciseRecurrence");
            RaisePropertyChangedEvent("ExerciseTimeUnits");
            RaisePropertyChangedEvent("IsUsedInRoster");
            RaisePropertyChangedEvent("IsWeekly");
            RaisePropertyChangedEvent("IsMonthly");
        }

        private void CloseDialogTrue()
        {
            DialogResult = true;
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
