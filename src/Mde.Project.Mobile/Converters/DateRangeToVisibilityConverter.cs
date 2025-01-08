using System;
using System.Globalization;
using Utilities;

namespace Mde.Project.Mobile.Converters
{
    public class DateRangeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
                var currentDate = DateTime.Now;
                var year = currentDate.Year;
                var startDate = new DateTime(year, 12, 1);
                var endDate = new DateTime(year, 1, GlobalConstants.EndDateSnow);
       
                if (currentDate.Month == 1)
                {
                    startDate = new DateTime(year - 1, 12, 1);
                    endDate = new DateTime(year, 1, GlobalConstants.EndDateSnow);
                }
                else if (currentDate.Month == 12)
                {
                    startDate = new DateTime(year, 12, 1);
                    endDate = new DateTime(year + 1, 1, GlobalConstants.EndDateSnow);
                }

                bool isWithinDateRange = currentDate >= startDate && currentDate <= endDate;

                return isWithinDateRange ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}