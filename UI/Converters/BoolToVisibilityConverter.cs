using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = value is bool boolean && boolean;
            if (Invert)
                isVisible = !isVisible;

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return Invert ? visibility != Visibility.Visible : visibility == Visibility.Visible;
            }

            return false;
        }
    }
}
