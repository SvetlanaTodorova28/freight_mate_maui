using Mde.Project.Mobile.Domain.Services.Interfaces;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class SnowVisibilityService : ISnowVisibilityService
{
    private readonly IPreferencesService _preferencesService;

    public SnowVisibilityService(IPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
    }

    public bool DetermineSnowVisibility()
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
        bool showSnowflakes = _preferencesService.GetBoolean("SnowEnabled", isWithinDateRange);

        if (currentDate > endDate)
        {
            _preferencesService.SetBoolean("SnowEnabled", false);
            showSnowflakes = false;
        }

        return showSnowflakes;
    }
}
