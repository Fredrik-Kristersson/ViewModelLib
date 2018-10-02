using System;
using System.Windows.Input;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ViewModelLib
{
    public class MyCommand : ICommand
    {
        private readonly Dispatcher _dispatcher;
        private readonly Action<object> execute;
        private readonly Func<bool> canExecute;

        public MyCommand(Action<object> execute)
        {
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
            }
            else
            {
                //this is useful for unit tests where there is no application running
                _dispatcher = Dispatcher.CurrentDispatcher;
            }

            Debug.Assert(_dispatcher != null);

            this.execute = execute;
            //CanExecuteChanged += delegate { };
        }

        public MyCommand(Action<object> execute, Func<bool> canExecute) :
            this(execute)
        {            
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected virtual void OnCanExecuteChanged()
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke((ThreadStart)OnCanExecuteChanged, DispatcherPriority.Normal);
            }
            else
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }


        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if(canExecute != null)
            {
                return canExecute();
            }
            return true;
        }

        //public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        

        #endregion
    }
}
