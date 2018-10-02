using System.Windows;
using System.Windows.Input;

namespace ViewModelLib.AttachedProperties
{
	public class MouseBehaviour
	{
		public static readonly DependencyProperty MouseDownCommandProperty =
			DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand),
			typeof(MouseBehaviour), new FrameworkPropertyMetadata(
			MouseDownCommandChanged));

		private static void MouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement element = (FrameworkElement)d;

			element.MouseDown += element_MouseDown;
		}

		static void element_MouseDown(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement element = (FrameworkElement)sender;

			ICommand command = GetMouseDownCommand(element);

			command.Execute(e);
		}

		public static void SetMouseDownCommand(UIElement element, ICommand value)
		{
			element.SetValue(MouseDownCommandProperty, value);
		}

		public static ICommand GetMouseDownCommand(UIElement element)
		{
			return (ICommand)element.GetValue(MouseDownCommandProperty);
		}
	}
}