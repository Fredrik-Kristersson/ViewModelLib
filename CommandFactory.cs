using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace ViewModelLib
{
    [Export(typeof(ICommandFactory))]
    public class CommandFactory : ICommandFactory
    {
        public ICommand CreateCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            return new MyCommand(execute, canExecute);
        }
    }
}
