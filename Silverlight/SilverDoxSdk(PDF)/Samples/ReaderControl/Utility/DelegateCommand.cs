using System;
using System.Windows.Input;

namespace PDFTron.SilverDox.Samples.Utility
{
    public class DelegateCommand : ICommand
    {
        private Predicate<Object> _canExecute;
        private Action<Object> _executeAction;
        public event EventHandler CanExecuteChanged;

        #region Constructors
        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, null)
        {
        }

        public DelegateCommand(Action<object> executeAction, Predicate<Object> canExecute)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;

        }
        #endregion

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public override string ToString()
        {
            string actionName = "null";
            string predicateName = "null";

            if (_executeAction != null && _executeAction.Method != null)
                actionName = _executeAction.Method.Name;

            if (_canExecute != null && _canExecute.Method != null)
                predicateName = _canExecute.Method.Name;

            return "[" + actionName + ", " + predicateName + "]";
        }
    }
}