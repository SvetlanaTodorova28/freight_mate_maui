using System.Globalization;

namespace Mde.Project.Mobile.Converters;

public class BoolToDangerousColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool) throw new ArgumentException("a boolean value must be supplied");

        bool dangerous = (bool)value;

        Application.Current.Resources.TryGetValue("WarningRed", out object dangerousColor);
        Application.Current.Resources.TryGetValue("Gray300", out object notDangerousColor);
        Color fallback = Color.FromRgb(0, 0, 0);

        if (dangerous) return (dangerousColor as Color) ?? fallback;
        else return (notDangerousColor as Color) ?? fallback;
    }
    
    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
