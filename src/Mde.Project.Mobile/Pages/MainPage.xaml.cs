using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Service;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class MainPage : ContentPage{
    private readonly IUiService _uiService;
    private readonly 
        ICargoService _cargoService;
    public MainPage(IUiService uiService, ICargoService cargoService)
    {
        InitializeComponent();
        _uiService = uiService;
        _cargoService = cargoService;
    }

    private async void CrtAccount_OnClicked(object? sender, EventArgs e){
        await Navigation.PushAsync(new AppUserRegisterPage(_uiService, _cargoService));
    }

    private async void Login_OnClicked(object? sender, EventArgs e){
        var loginViewModel = new HomeViewModel(_cargoService, _uiService)
        {
            UserName = "",
            Password = ""
        };

        var loginPage = new HomePage(loginViewModel, _uiService, _cargoService)
        {
            BindingContext = loginViewModel
        };

        await Navigation.PushAsync(loginPage);
    }
}