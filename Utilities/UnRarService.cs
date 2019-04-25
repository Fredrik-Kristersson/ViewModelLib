using log4net;
using SharpCompress.Archives;
using SharpCompress.Common;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModelLib.Utilities
{
	[Export(typeof(IUnRarService))]
	public class UnRarService : IUnRarService
	{
		private readonly IFileService fileService;
		private ILog log;

		[ImportingConstructor]
		public UnRarService(ILogger logger, IFileService fileService)
		{
			log = logger.GetLogger(GetType());
			this.fileService = fileService;
		}

		public async Task UnrarFolderAsync(
			string path,
			string destinationFolder,
			CancellationToken ct,
			double progressMaximum = 0,
			IProgress<string> progressText = null,
			IProgress<double> progressValue = null)
		{
			if (Directory.Exists(path))
			{
				var percentageForEachRar =
				progressMaximum / fileService.CountFilesWithExtension(path, "rar", true);
				await UnrarFolderRecursiveAsync(
					path, destinationFolder, ct, percentageForEachRar, progressText, progressValue);
			}

			log.Debug($"Finished unrar...");
			progressText?.Report("Extract completed successfully");
		}

		private async Task UnrarFolderRecursiveAsync(
			string path,
			string destinationFolder,
			CancellationToken ct,
			double percentageForItemInFolder = 0,
			IProgress<string> progressText = null,
			IProgress<double> progressValue = null)
		{
			ct.ThrowIfCancellationRequested();

			log.Debug($"percentageForItemInFolder: {percentageForItemInFolder}");
			if (Directory.Exists(path))
			{
				var info = new DirectoryInfo(path);
				foreach (var file in info.GetFiles("*.rar"))
				{
					await UnrarFileAsync(
						file.FullName,
						destinationFolder,
						ct,
						percentageForItemInFolder,
						progressText,
						progressValue);
				}

				foreach (var dir in info.GetDirectories())
				{
					await UnrarFolderRecursiveAsync(
						dir.FullName,
						destinationFolder,
						ct,
						percentageForItemInFolder,
						progressText,
						progressValue);
				}
			}
		}

		public Task UnrarFileAsync(
			string filePath,
			string destinationFolder,
			CancellationToken ct,
			double progressMaximumForItem = 0,
			IProgress<string> progressText = null,
			IProgress<double> progressValue = null)
		{
			using (var entries = ArchiveFactory.Open(filePath))
			{
				var entryProgressStep = progressMaximumForItem / entries.Entries.Count();
				var destDir =
					!string.IsNullOrEmpty(destinationFolder) && Directory.Exists(destinationFolder)
					? destinationFolder : new FileInfo(filePath).DirectoryName;
				foreach (var entry in entries.Entries)
				{
					ct.ThrowIfCancellationRequested();
					progressText?.Report($"Extracting {entry.Key}");
					log.Debug($"Unraring {entry.Key}");

					try
					{
						entry.WriteToDirectory(destDir, new ExtractionOptions() { Overwrite = false });
					}
					catch (IOException ioex)
					{
						// If rar is already extracted this exception is thrown.
						log.Info($"Exception message: [{ioex.Message}]");
					}

					progressValue?.Report(entryProgressStep);
					progressText?.Report($"Extracted {entry.Key}");
				}
			}

			return Task.CompletedTask;
		}
	}
}
