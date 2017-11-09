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
        //public ObservableCollection<ExerciseItem> RosterItems { get; set; }
        public ObservableCollection<RosterListItemViewModel> RosterItems { get; set; }

        public ICommand CloseDialogTrueCommand { get { return new DelegateCommand(CloseDialogTrue); } }

        public EditRosterViewModel(List<ExerciseItem> passedExerciseItems)
        {
            //RosterItems = new ObservableCollection<ExerciseItem>();
            RosterItems = new ObservableCollection<RosterListItemViewModel>();

            foreach (var item in passedExerciseItems)
            {
                RosterItems.Add(new RosterListItemViewModel()
                {
                    IsUsedInRoster = item.IsUsedInRoster,
                    ExerciseName = item.ExerciseName,
                    IsSets = item.IsSets,
                    RequiredSetsCount = item.RequiredSetsCount,
                    ShownCount = item.ShownCount,
                    GUIDID = item.GUIDID,
                });
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
