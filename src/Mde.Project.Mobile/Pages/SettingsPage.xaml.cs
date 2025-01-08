

namespace Mde.Project.Mobile.Pages;

public partial class SettingsPage : ContentPage{
    private readonly SettingsViewModel _settingsViewModel;

    public SettingsPage(SettingsViewModel settingsViewModel){
        _settingsViewModel = settingsViewModel;
        InitializeComponent();
        BindingContext = _settingsViewModel;
    }

}