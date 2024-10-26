using System.Globalization;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

namespace Mde.Project.Mobile.Converters;

public class UserDisplayConverter : IValueConverter{
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture){
        if (value is AppUserResponseDto user){
            return $"{user.FirstName} - {user.AccessLevelType?.Name}";
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture){
        throw new NotImplementedException();
    }
}