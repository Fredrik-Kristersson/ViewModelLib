using log4net;
using log4net.Config;
using log4net.Appender;
using System;
using System.IO;
using log4net.Layout;

namespace ViewModelLib
{
	public static class YeOldeLogger
	{

		private static readonly ILog log = LogManager.GetLogger(typeof(YeOldeLogger));

		/// <summary>
		/// Initialize the logger with a log4net xml config file.
		/// Debug mode will automatically append to console.
		/// </summary>
		/// <param name="configFilePath"></param>
		public static void InitializeLogger(string configFilePath)
		{
			XmlConfigurator.Configure(new FileInfo(configFilePath));
			log.Info("Loading log configuration @ " + configFilePath);
		}

		public static void InitializeDebugConsoleLogger() 
		{			
			ConsoleAppender appender = new ConsoleAppender();
			appender.Threshold = log4net.Core.Level.Debug;
			appender.Layout = new PatternLayout("%level %date %thread %logger - %message%newline");
			BasicConfigurator.Configure(appender);
			log.Info("Logging @ (debug)");
		}

		public static ILog GetLogger(Type classType)
		{
			log.DebugFormat("Getting logger for [{0}]", classType.Name);
			return LogManager.GetLogger(classType);
		}
	}
}
