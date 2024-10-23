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
    private readonly AppUserRegisterViewModel _userRegisterViewModel;

    private readonly
        IWebCargoService _cargoService;

    public LoginPage(LoginViewModel loginViewModel, IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel){

        InitializeComponent();
        BindingContext = loginViewModel;
        _uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        _authenticationServiceMobile = authenticationServiceMobile; 
       
    }

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync(new WelcomePage(_uiService, _authenticationServiceMobile, _userRegisterViewModel));
    }
}