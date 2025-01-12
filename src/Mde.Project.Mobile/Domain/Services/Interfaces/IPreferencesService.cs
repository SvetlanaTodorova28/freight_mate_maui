namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IPreferencesService
{
    bool GetBoolean(string key, bool defaultValue);
    void SetBoolean(string key, bool value);

    
}
