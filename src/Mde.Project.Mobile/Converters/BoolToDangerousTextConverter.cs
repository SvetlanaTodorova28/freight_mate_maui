using System.Globalization;
using System.Net.Mime;

namespace Mde.Project.Mobile.Converters;

public class BoolToDangerousTextConverter:IValueConverter{
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool)
            throw new ArgumentException("An bool value was not supplied. Cannot convert.");

        bool dangerous = (bool)value;

        if (!dangerous) return "The cargo does not contain dangerous products";
        return "The cargo contains dangerous products";
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    
    
}