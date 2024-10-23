using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class AppUserRegisterPage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
   
    public AppUserRegisterPage(IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
        AppUserRegisterViewModel userRegisterViewModel){
        InitializeComponent();
        _uiService = uiService;
       BindingContext =  _userRegisterViewModel = userRegisterViewModel;
        this.authenticationServiceMobile = authenticationServiceMobile;
       
    }

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync( new WelcomePage(_uiService, authenticationServiceMobile,_userRegisterViewModel));
    }
    
    protected override void OnAppearing()
    {
        _userRegisterViewModel?.OnAppearingCommand.Execute(null);
    }

    private void CreateAccount_OnClicked(object? sender, EventArgs e){
        _userRegisterViewModel?.RegisterCommand.Execute(null);
    }
}