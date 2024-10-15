using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class LoginPage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;

    private readonly
        IWebCargoService _cargoService;

    public LoginPage(LoginViewModel loginViewModel, IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile){

        InitializeComponent();
        BindingContext = loginViewModel;
        _uiService = uiService;
        _authenticationServiceMobile = authenticationServiceMobile; 
       
    }

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync(new WelcomePage(_uiService, _authenticationServiceMobile));
    }
}