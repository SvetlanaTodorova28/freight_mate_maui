using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;

namespace Mde.Project.Mobile.ViewModels;

public class OptionsViewModel: ObservableObject{
    private readonly ISnowVisibilityService _snowVisibilityService;
    
    public OptionsViewModel(ISnowVisibilityService snowVisibilityService){
      _snowVisibilityService = snowVisibilityService;
       
        UpdateSnowVisibility();
        InitializeSubscriptionSnow();
    }
    private bool _snowVisibility;
    public bool SnowVisibility
    {
        get => _snowVisibility;
        private set => SetProperty(ref _snowVisibility, value);
    }

    private void InitializeSubscriptionSnow(){
        MessagingCenter.Subscribe<SettingsViewModel, bool>(this, "SnowToggleChanged",  (sender, isEnabled) =>
        {
            UpdateSnowVisibility();
        });
    }
    
    public void UpdateSnowVisibility()
    {
        SnowVisibility = _snowVisibilityService.DetermineSnowVisibility();
    }
}
