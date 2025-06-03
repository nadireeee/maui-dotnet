using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MovieApp.Maui.Converters;

public class SelectedFilterToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var selected = value?.ToString();
        var current = parameter?.ToString();
        return selected == current ? "#512BD4" : "#232323";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
} 