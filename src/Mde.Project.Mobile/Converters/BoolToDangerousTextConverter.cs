using System.Globalization;


namespace Mde.Project.Mobile.Converters
{
    public class BoolToDangerousTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is bool dangerous)
                {
                    return dangerous ? "Yes" : "No";
                }
                
                System.Diagnostics.Debug.WriteLine("Invalid input: expected a boolean value.");
                return "Unknown"; 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in BoolToDangerousTextConverter: {ex.Message}");
                return "Error"; 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine("ConvertBack is not implemented for BoolToDangerousTextConverter.");
            throw new NotSupportedException("ConvertBack is not supported.");
        }
    }
}