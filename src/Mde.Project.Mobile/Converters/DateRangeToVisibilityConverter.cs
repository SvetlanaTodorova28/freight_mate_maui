using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Mde.Project.Mobile.Converters
{
    public class DateRangeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime currentDate)
            {
                
                DateTime startDate = new DateTime(2024, 12, 01); 
                DateTime endDate = new DateTime(2025, 1, 10);   
               
                if (currentDate >= startDate && currentDate <= endDate)
                {
                    return true; 
                }
            }

            return false; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}