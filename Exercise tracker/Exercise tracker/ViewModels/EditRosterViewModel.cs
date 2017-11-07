using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmDialogs;
using Exercise_tracker.Helpers;
using Exercise_tracker.Classes;

namespace Exercise_tracker.ViewModels
{
    class EditRosterViewModel : ObservableObject, IModalDialogViewModel
    {
        public ObservableCollection<ExerciseItem> AllExerciseItemsToAdd { get { return _allExerciseItems; } }

        private readonly ObservableCollection<ExerciseItem> _allExerciseItems = new ObservableCollection<ExerciseItem>();

        public EditRosterViewModel(List<ExerciseItem> passedExerciseItems)
        {
            foreach (var item in passedExerciseItems)
            {
                AllExerciseItemsToAdd.Add(item);
            }
        }

        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            private set
            {
                dialogResult = value;
                RaisePropertyChangedEvent("DialogResult");
            }
        }
    }
}
