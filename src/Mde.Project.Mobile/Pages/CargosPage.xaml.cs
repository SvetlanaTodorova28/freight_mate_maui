
using Mde.Project.Mobile.ViewModels;


namespace Mde.Project.Mobile.Pages;

public partial class CargosPage : ContentPage{
    
   
    public CargosPage(HomeViewModel homeViewModel){
        InitializeComponent();
        BindingContext = homeViewModel;
        
    }
    private async void Add_Cargo_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/createCargo");
    }

    private async  void Button1_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/infoCargo");
    }
}