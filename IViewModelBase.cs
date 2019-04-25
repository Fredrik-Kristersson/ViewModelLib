using System.ComponentModel;
using System.Threading.Tasks;

namespace ViewModelLib
{
    public interface IViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        Task OnLoadAsync();
        Task OnUnloadAsync();
    }
}
