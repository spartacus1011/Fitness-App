using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Exercise_tracker.Classes;
using Exercise_tracker.Helpers;
using MvvmDialogs;

namespace Exercise_tracker.ViewModels
{
    public class OneTimeExerciseViewModel:ObservableObject, IModalDialogViewModel
    {
        public ICommand CloseDialogTrueCommand { get { return new DelegateCommand(CloseDialogTrue); } }

        public Dictionary<string, ExerciseItem> OneTimeAddComboboxItems { get; set; }
        private KeyValuePair<string, ExerciseItem> selectedExercise;
        public KeyValuePair<string, ExerciseItem> SelectedExercise
        {
            get { return selectedExercise; }
            set
            {
                if (value.Key != selectedExercise.Key)
                {
                    selectedExercise = value;

                }

            }
        }


        public OneTimeExerciseViewModel(List<ExerciseItem> allExerciseItems)
        {
            OneTimeAddComboboxItems = new Dictionary<string, ExerciseItem>();

            foreach (var exercise in allExerciseItems)
            {
                OneTimeAddComboboxItems.Add(exercise.ExerciseName, exercise);
            }
        }


        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            private set //this should do the same as the other thing
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
