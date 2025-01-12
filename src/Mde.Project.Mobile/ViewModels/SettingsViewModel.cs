using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Domain.Services.Interfaces;

public class SettingsViewModel : ObservableObject
{
    private IPreferencesService _preferencesService;
    private bool _snowEnabled;

    // Constructor Injection
    public SettingsViewModel(IPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
        SnowEnabled = _preferencesService.GetBoolean("SnowEnabled", false);
    }

    public bool SnowEnabled
    {
        get => _snowEnabled;
        set
        {
            if (SetProperty(ref _snowEnabled, value))
            {
                _preferencesService.SetBoolean("SnowEnabled", value);
                MessagingCenter.Send(this, "SnowToggleChanged", _snowEnabled);
            }
        }
    }
}