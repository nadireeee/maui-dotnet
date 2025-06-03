using System.Globalization;

namespace MovieApp.Maui.Converters;

public class BoolToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isSearching)
        {
            return isSearching ? "no_results.png" : "no_movies.png";
        }
        return "no_movies.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 