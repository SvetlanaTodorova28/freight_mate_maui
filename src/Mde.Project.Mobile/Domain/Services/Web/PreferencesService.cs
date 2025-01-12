using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class PreferencesService : IPreferencesService
{
   
    public bool GetBoolean(string key, bool defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }

    public void SetBoolean(string key, bool value)
    {
        Preferences.Set(key, value);
    }


}
