using System;
using System.Windows.Input;

namespace ViewModelLib
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(Action<object> execute, Func<bool> canExecute = null);
    }
}