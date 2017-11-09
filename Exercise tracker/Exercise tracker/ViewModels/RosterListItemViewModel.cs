using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Exercise_tracker.Helpers;

namespace Exercise_tracker.ViewModels
{
    class RosterListItemViewModel: ObservableObject
    {
        public ICommand ToggleAddToRosterCommand { get { return new DelegateCommand(ToggleAddToRoster); } }

        public bool IsUsedInRoster { get; set; }
        public int RequiredSetsCount { get; set; }
        public bool IsSets { get; set; }
        public string ExerciseName { get; set; }
        public string ShownCount { get; set; }
        public string GUIDID { get; set; }

        private void ToggleAddToRoster()
        {
            IsUsedInRoster = !IsUsedInRoster;
            RaisePropertyChangedEvent("IsUsedInRoster");
        }
    }
}
