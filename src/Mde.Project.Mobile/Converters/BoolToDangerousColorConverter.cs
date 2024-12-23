using System.Globalization;

namespace Mde.Project.Mobile.Converters;

public class BoolToDangerousColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (value is not bool dangerous)
            {
               
                System.Diagnostics.Debug.WriteLine("Invalid input: expected a boolean value.");
            
                return GetFallbackColor();
            }

            Application.Current.Resources.TryGetValue("Warning", out object dangerousColor);
            Application.Current.Resources.TryGetValue("White", out object notDangerousColor);
            Color fallback = GetFallbackColor();

            return dangerous ? (dangerousColor as Color) ?? fallback : (notDangerousColor as Color) ?? fallback;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in converter: {ex.Message}");
            return GetFallbackColor();
        }
    }
    
    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    private Color GetFallbackColor()
    {
        return Color.FromRgb(0, 0, 0); 
    }
}
