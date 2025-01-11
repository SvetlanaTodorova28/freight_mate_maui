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

            if (endDate < DateTime.Today){
                Preferences.Set("SnowEnabled", false);
            }

            bool isWithinDateRange = currentDate >= startDate && currentDate <= endDate;
            bool showSnowflakes = Preferences.Get("SnowEnabled", false);

            return isWithinDateRange || showSnowflakes;
        }
    }
}