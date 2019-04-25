using System.Reflection;

namespace ViewModelLib
{
	public interface IDatabaseStarter
	{
		void SetupDatabase(string databaseFilePath, string databaseConfigFilePath, Assembly asm);
	}
}
