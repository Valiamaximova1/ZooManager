using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors; 
using System.Windows.Media;
using UI.ViewModels; 

namespace UI.Behaviors
{
    public class DataGridRightClickPopupBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseRightButtonDown += OnMouseRightButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseRightButtonDown -= OnMouseRightButtonDown;
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = AssociatedObject;
            var visualHit = e.OriginalSource as DependencyObject;

            while (visualHit != null && !(visualHit is DataGridRow))
                visualHit = VisualTreeHelper.GetParent(visualHit);

            if (visualHit is DataGridRow row)
            {
                row.IsSelected = true;
                dataGrid.SelectedItem = row.Item;

                if (dataGrid.DataContext is EventsViewModel vm)
                    vm.IsPopupOpen = true;
            }
        }
    }

}
