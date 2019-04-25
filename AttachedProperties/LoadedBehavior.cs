using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using log4net;

namespace ViewModelLib.AttachedProperties
{
    public static class LoadedBehavior
    {
        private static readonly IDictionary<FrameworkElement, IViewModelBase> elementToViewModel =
            new ConcurrentDictionary<FrameworkElement, IViewModelBase>();

        private static readonly ILog log = LogManager.GetLogger(typeof(LoadedBehavior));

        public static DependencyProperty ViewModelConnectedProperty
            = DependencyProperty.RegisterAttached(
                "ViewModelConnected",
                typeof(IViewModelBase),
                typeof(LoadedBehavior),
                new PropertyMetadata(null, OnViewModelConnectedChanged));

        public static IViewModelBase GetViewModelConnected(DependencyObject depObj)
        {
            return (IViewModelBase)depObj.GetValue(ViewModelConnectedProperty);
        }

        public static void SetViewModelConnected(
            DependencyObject depObj,
            IViewModelBase value)
        {
            depObj.SetValue(ViewModelConnectedProperty, value);
        }

        private static void OnViewModelConnectedChanged
            (DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (depObj is FrameworkElement fe && e.NewValue is IViewModelBase vm)
            {
                elementToViewModel.Add(fe, vm);
                fe.Loaded += OnLoaded;
                fe.Unloaded += OnUnloaded;
            }
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                if (elementToViewModel.TryGetValue(fe, out IViewModelBase vm))
                {
                    var unloadResult = Task.Run(async () => await vm.OnUnloadAsync());
                    if (unloadResult.IsFaulted)
                    {
                        log.Error($"Unload faulted for [{vm.GetType()}]", unloadResult.Exception);
                    }

                    fe.Loaded -= OnLoaded;
                    fe.Unloaded -= OnUnloaded;
                    elementToViewModel.Remove(fe);
                }
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                if (elementToViewModel.TryGetValue(fe, out IViewModelBase vm))
                {
                    var loadResult = Task.Run(async () => await vm.OnLoadAsync());
                    if (loadResult.IsFaulted)
                    {
                        log.Error($"Load faulted for [{vm.GetType()}]", loadResult.Exception);
                    }
                }
            }
        }

        //private static void VmOnCloseRequest(object sender, EventArgs e)
        //{
        //    if (!(sender is ViewModelBase vm) || !viewModelToElement.TryGetValue(vm, out var fe))
        //    {
        //        return;
        //    }

        //    fe.Loaded -= vm.OnLoaded;
        //    fe.Loaded -= vm.OnUnloaded;
        //    if (fe is Window window)
        //    {
        //        window.Close();
        //    }
        //    else
        //    {
        //        viewModelToElement.Remove(vm);
        //    }
        //}
    }
}
