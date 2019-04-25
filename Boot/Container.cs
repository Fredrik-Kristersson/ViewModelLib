using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace ViewModelLib.Boot
{
	[Export(typeof(IContainer))]
	public class Container : IContainer
	{
		private readonly ILogger logger;

		[ImportingConstructor]
		public Container(ILogger logger)
		{
			this.logger = logger;
		}

		public CompositionContainer CompositionContainer { get; set; }

		public T GetExport<T>()
		{
			return CompositionContainer.GetExport<T>().Value;
		}
	}
}
