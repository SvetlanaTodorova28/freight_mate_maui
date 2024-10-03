using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.ViewModels;


namespace Mde.Project.Mobile.Pages;

public partial class HomePage : ContentPage{
    private readonly IUiService _uiService;
    private readonly 
        ICargoService _cargoService;
    
    public HomePage(HomeViewModel homeViewModel,IUiService uiService, ICargoService cargoService){
        
        InitializeComponent();
        BindingContext = homeViewModel;
        _uiService = uiService;
        _cargoService = cargoService;
    }

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync( new MainPage(_uiService, _cargoService));
    }
}