using System;
using Utilities;

namespace Mde.Project.Mobile.Helpers
{
    public class SnowVisibilityHelper
    {
        public static bool DetermineSnowVisibility()
        {
            var currentDate = DateTime.Now;
            var year = currentDate.Year;
            DateTime startDate, endDate;

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
            else
            {
                return false;
            }

           
            bool isWithinDateRange = currentDate >= startDate && currentDate <= endDate;
            
            bool showSnowflakes = Preferences.Get("SnowEnabled", isWithinDateRange);

          
            if (currentDate > endDate)
            {
                Preferences.Set("SnowEnabled", false);
                showSnowflakes = false;
            }

            return showSnowflakes;
        }
    }
}