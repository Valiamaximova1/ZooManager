using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace UI.Behaviors
{
    public class SelectedItemsBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(SelectedItemsBehavior),
                new PropertyMetadata(null, OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.SelectionChanged -= ListBox_SelectionChanged;
                listBox.SelectionChanged += ListBox_SelectionChanged;

                var boundList = e.NewValue as IList;
                if (boundList == null) return;

                // Изчистваме само ако има разлика
                if (!listBox.SelectedItems.Cast<object>().SequenceEqual(boundList.Cast<object>()))
                {
                    listBox.SelectedItems.Clear();

                    foreach (var item in boundList)
                    {
                        if (listBox.Items.Contains(item))
                            listBox.SelectedItems.Add(item);
                    }
                }
            }
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var selectedItems = GetSelectedItems(listBox);

            if (selectedItems == null) return;

            foreach (var item in e.RemovedItems)
                selectedItems.Remove(item);

            foreach (var item in e.AddedItems)
                if (!selectedItems.Contains(item))
                    selectedItems.Add(item);
        }
    }
}
