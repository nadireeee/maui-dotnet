using System.Globalization;

namespace MovieApp.Maui.Converters;

public class NullToNoImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string posterPath && !string.IsNullOrWhiteSpace(posterPath))
        {
            return posterPath;
        }
        return "no_image.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 