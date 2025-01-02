

using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class OptionsPage : ContentPage{
    
    private readonly OptionsViewModel _optionsViewModel;
    public OptionsPage( OptionsViewModel optionsViewModel){
        BindingContext = _optionsViewModel = optionsViewModel;
        InitializeComponent();
    }
    private async void NavigateToAbout(object? sender, EventArgs e){
        await Shell.Current.GoToAsync(nameof(AboutPage));
    }
    
    private async void NavigateToSettings(object? sender, EventArgs e){
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
    

   
}