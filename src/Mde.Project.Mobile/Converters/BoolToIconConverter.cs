using System.Globalization;

namespace Mde.Project.Mobile.Converters;

public class BoolToIconConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool)
            throw new ArgumentException("An bool value was not supplied. Cannot convert.");

        bool dangerous = (bool)value;
        if (dangerous) return "biohazard.png";
        return " ";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
