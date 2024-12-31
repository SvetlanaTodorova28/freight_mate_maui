namespace Mde.Project.Mobile.Helpers;

public class SnowVisibilityHelper{
    public static bool DetermineSnowVisibility()
    {
        var currentDate = DateTime.Now;
        var year = currentDate.Year;
        var startDate = new DateTime(year, 12, 1);
        var endDate = new DateTime(year, 1, 10);
       
        if (currentDate.Month == 1)
        {
            endDate = new DateTime(year, 1, 10);
        }
        else if (currentDate.Month == 12)
        {
            startDate = new DateTime(year, 12, 1);
            endDate = new DateTime(year + 1, 1, 10);
        }

        bool isWithinDateRange = currentDate >= startDate && currentDate <= endDate;
        bool showSnowflakes = Preferences.Get("SnowEnabled", true);

        return isWithinDateRange || showSnowflakes;
    }
}