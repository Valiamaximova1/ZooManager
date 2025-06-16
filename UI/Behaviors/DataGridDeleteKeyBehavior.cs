using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace UI.Behaviors
{
    public class DataGridDeleteKeyBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DataGridDeleteKeyBehavior));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        //гледа view-то в което е извикано и чака за натискане на бутон
        protected override void OnAttached()
        {
            AssociatedObject.PreviewKeyDown += OnKeyDown;
        }
        //задължително се пише за да не се допуска memory leaks
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= OnKeyDown;
        }
        //тук се проверява дали е натиснат точно delete и изпълнява подадената от view команда
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && Command?.CanExecute(null) == true)
                Command.Execute(null);
        }
    }
}
