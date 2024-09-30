using Mde.Project.Mobile.ViewModels;


namespace Mde.Project.Mobile.Pages;

public partial class HomePage : ContentPage{
    
    
    public HomePage(HomeViewModel homeViewModel){
        InitializeComponent();
        BindingContext = homeViewModel;
    }
    private async void CreateAccount_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/appuserRegister");
    }
    private async void OnLoginClicked(object sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/CagosPage");
    }

  
}