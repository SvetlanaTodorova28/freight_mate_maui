using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Helpers;

namespace Mde.Project.Mobile.ViewModels;

public class OptionsViewModel: ObservableObject{
    
    public OptionsViewModel(){
        UpdateSnowVisibility();
       
        MessagingCenter.Subscribe<SettingsViewModel, bool>(this, "SnowToggleChanged", (sender, isEnabled) =>
        {
            UpdateSnowVisibility();
        });
    }
    private bool _snowVisibility;
    public bool SnowVisibility
    {
        get => _snowVisibility;
        private set => SetProperty(ref _snowVisibility, value);
    }

    public void UpdateSnowVisibility()
    {
        SnowVisibility = SnowVisibilityHelper.DetermineSnowVisibility();
    }
}
