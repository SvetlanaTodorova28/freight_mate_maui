

namespace Mde.Project.Mobile.Pages;

public partial class OptionsPage : ContentPage{
    public OptionsPage(){
        InitializeComponent();
    }
    private async void NavigateToAbout(object? sender, EventArgs e){
        await Shell.Current.GoToAsync(nameof(AboutPage));
    }
    
    private async void NavigateToSettings(object? sender, EventArgs e){
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
    

   
}