using CommunityToolkit.Mvvm.ComponentModel;


public class SettingsViewModel : ObservableObject
{
    public SettingsViewModel()
    {
        SnowEnabled = Preferences.Get("SnowEnabled", true);
    }

    private bool _snowEnabled;
    public bool SnowEnabled
    {
        get => _snowEnabled;
        set
        {
            if (SetProperty(ref _snowEnabled, value))
            {
               
                Preferences.Set("SnowEnabled", value);
                MessagingCenter.Send<SettingsViewModel, bool>(this, "SnowToggleChanged", true);
            }
        }
    }
}