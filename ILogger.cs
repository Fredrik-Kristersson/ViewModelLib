using System;
using log4net;

namespace ViewModelLib
{
    public interface ILogger
    {
        ILog GetLogger(Type classType);
        void InitializeDebugConsoleLogger();
        void InitializeLogger(string configFilePath);
    }
}