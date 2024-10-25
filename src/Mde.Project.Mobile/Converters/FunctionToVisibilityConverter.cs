using System.Globalization;
using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Converters;

public class FunctionToVisibilityConverter:IValueConverter{
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Function function && parameter is string targetFunction)
        {
           
            return function.ToString().Equals(targetFunction, StringComparison.OrdinalIgnoreCase) 
                   || function.ToString().Equals("Consignee", StringComparison.OrdinalIgnoreCase);
        }
        return false; 
    
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}