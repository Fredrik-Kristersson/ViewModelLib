using System;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModelLib.Utilities
{
	public interface IUnRarService
	{
		/// <summary>
		/// Extracts all rar files found in <paramref name="path"/> and its subfolders.
		/// Reports progress in increments, where the total of those increments will be 
		/// <paramref name="progressMaximum"/>
		/// </summary>
		Task UnrarFolderAsync(
		string path,
		string destinationFolder,
		CancellationToken ct,
		double progressMaximum = 0,
		IProgress<string> progressText = null,
		IProgress<double> progressValue = null);

		/// <summary>
		/// Extracts a rar file found in <paramref name="filePath"/>.
		/// Reports progress in increments, where the total of those increments will be 
		/// <paramref name="progressMaximum"/>
		/// </summary>
		Task UnrarFileAsync(
			string filePath,
			string destinationFolder,
			CancellationToken ct,
			double progressMaximumForItem = 0,
			IProgress<string> progressText = null,
			IProgress<double> progressValue = null);
	}
}
