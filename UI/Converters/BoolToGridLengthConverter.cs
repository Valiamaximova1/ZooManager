using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI.Converters
{
    public class BoolToGridLengthConverter : IValueConverter
    {
        // ConverterParameter: "WidthWhenFalse;WidthWhenTrue" – напр. "*;3*"
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string param && param.Contains(";"))
            {
                var parts = param.Split(';');
                bool isVisible = (bool)value;
                var selected = isVisible ? parts[1] : parts[0];
                return GridLengthConverter.ConvertFromString(selected) ?? new GridLength(1, GridUnitType.Star);
            }

            return new GridLength(1, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        private static readonly GridLengthConverter GridLengthConverter = new();
    }
}
