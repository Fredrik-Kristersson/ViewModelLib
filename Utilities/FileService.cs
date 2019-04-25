using System.ComponentModel.Composition;
using System.IO;

namespace ViewModelLib.Utilities
{
	[Export(typeof(IFileService))]
	public class FileService : IFileService
	{
		public int CountFilesWithExtension(string folderPath, string extension, bool includeSubDirs)
		{
			extension = extension.ToLowerInvariant();
			int sum = 0;
			if (Directory.Exists(folderPath))
			{
				var info = new DirectoryInfo(folderPath);
				sum += info.GetFiles($"*.{extension}").Length;
				if (includeSubDirs)
				{
					foreach (var dir in info.GetDirectories())
					{
						sum += CountFilesWithExtension(dir.FullName, extension, includeSubDirs);
					}
				}
			}

			return sum;
		}
	}
}
