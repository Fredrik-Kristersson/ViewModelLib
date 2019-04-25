namespace ViewModelLib.Utilities
{
	public interface IFileService
	{
		int CountFilesWithExtension(string folderPath, string extension, bool includeSubDirs);
	}
}
