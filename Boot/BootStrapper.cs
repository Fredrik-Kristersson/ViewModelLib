using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace ViewModelLib.Boot
{
	public static class BootStrapper
	{
		public static IContainer Compose(
			object startupOrigin,
			string logConfigFilePath = null,
			params Assembly[] otherAssemblies)
		{
			var catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog(typeof(BootStrapper).Assembly));
			catalog.Catalogs.Add(new AssemblyCatalog(startupOrigin.GetType().Assembly));
			foreach (var assembly in otherAssemblies)
			{
				catalog.Catalogs.Add(new AssemblyCatalog(assembly));
			}

			var compositionContainer = new CompositionContainer(catalog);

			try
			{
				compositionContainer.ComposeParts(startupOrigin);

				var container = compositionContainer.GetExport<IContainer>().Value;
				container.CompositionContainer = compositionContainer;
				if (!string.IsNullOrEmpty(logConfigFilePath))
				{
					var logger = compositionContainer.GetExport<ILogger>().Value;
					logger.InitializeLogger(logConfigFilePath);
				}

				return container;
			}
			catch (CompositionException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
