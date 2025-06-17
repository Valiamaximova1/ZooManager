using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using BusinessLayer.DTOs;

namespace UI.Converters
{
    //public class AnimalSelectionMultiConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var selectedAnimals = values[0] as ObservableCollection<AnimalDto>;
    //        var currentAnimal = values[1] as AnimalDto;

    //        if (selectedAnimals == null || currentAnimal == null)
    //            return false;

    //        return selectedAnimals.Contains(currentAnimal);
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        return null; // няма нужда от обратна посока
    //    }
    //}
}
