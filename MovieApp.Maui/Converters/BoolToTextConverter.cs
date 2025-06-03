using System.Globalization;

namespace MovieApp.Maui.Converters;

public class BoolToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isSearching)
        {
            return isSearching ? "No results found" : "No movies available";
        }
        return "No movies available";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 