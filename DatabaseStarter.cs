﻿using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace ViewModelLib
{
	[Export(typeof(IDatabaseStarter))]
	public class DatabaseStarter : IDatabaseStarter
	{
		public void SetupDatabase(string databaseFilePath, string databaseConfigFilePath, Assembly asm)
		{
			XmlConfigurationSource source = new XmlConfigurationSource(databaseConfigFilePath);

			ActiveRecordStarter.Initialize(asm, source);
			if (!DatabaseExists(databaseFilePath))
			{
				ActiveRecordStarter.CreateSchema();
			}
		}

		private bool DatabaseExists(string databaseName)
		{
			return File.Exists(databaseName);
		}
	}
}
