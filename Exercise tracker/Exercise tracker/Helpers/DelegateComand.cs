using System;
using System.Windows.Input;

namespace Exercise_tracker.Helpers
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action ExecuteAction)
        {
            _action = ExecuteAction;
        }

        public void Execute(object parameter = null) //the parameter should always be null. Any input data should be a private variable in the view model
        {
            _action();
        }

        public bool CanExecute(object parameter = null)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged; //dont really need this guy at all if im not using can execute
    }
}
