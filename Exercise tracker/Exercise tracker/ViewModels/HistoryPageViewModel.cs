using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MvvmDialogs;
using Exercise_tracker.Helpers;
using Exercise_tracker.Classes;

namespace Exercise_tracker.ViewModels
{
    public class HistoryPageViewModel : ObservableObject, IModalDialogViewModel
    {
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
