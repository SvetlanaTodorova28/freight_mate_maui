using System.Globalization;


namespace Mde.Project.Mobile.Converters
{
    public class BoolToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is bool dangerous)
                {
                    return dangerous ? "danger_container.png" : " "; 
                }
                
                System.Diagnostics.Debug.WriteLine("Invalid input: expected a boolean value.");
                return " "; 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in BoolToIconConverter: {ex.Message}");
                return " "; 
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            System.Diagnostics.Debug.WriteLine("ConvertBack is not implemented for BoolToIconConverter.");
            throw new NotSupportedException("ConvertBack is not supported.");
        }
    }
}