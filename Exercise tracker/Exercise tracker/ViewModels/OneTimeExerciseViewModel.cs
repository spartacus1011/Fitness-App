using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_tracker.Helpers;
using MvvmDialogs;

namespace Exercise_tracker.ViewModels
{
    public class OneTimeExerciseViewModel:ObservableObject, IModalDialogViewModel
    {

        public OneTimeExerciseViewModel()
        {

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
    }
}
