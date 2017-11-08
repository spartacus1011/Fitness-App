using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmDialogs;
using Exercise_tracker.Helpers;
using Exercise_tracker.Classes;
using System.Windows.Input;

namespace Exercise_tracker.ViewModels
{
    class EditRosterViewModel : ObservableObject, IModalDialogViewModel
    {
        public ObservableCollection<ExerciseItem> RosterItems { get; set; }
        public ICommand CloseDialogTrueCommand { get { return new DelegateCommand(CloseDialogTrue); } }

        public EditRosterViewModel(List<ExerciseItem> passedExerciseItems)
        {
            RosterItems = new ObservableCollection<ExerciseItem>();

            foreach (var item in passedExerciseItems)
            {
                RosterItems.Add(item);
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

        private void CloseDialogTrue()
        {
            DialogResult = true;
        }
    }
}
