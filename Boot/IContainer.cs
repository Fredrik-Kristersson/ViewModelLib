using System.ComponentModel.Composition.Hosting;

namespace ViewModelLib.Boot
{
	public interface IContainer
	{
		T GetExport<T>();

		CompositionContainer CompositionContainer { get; set; }
	}
}