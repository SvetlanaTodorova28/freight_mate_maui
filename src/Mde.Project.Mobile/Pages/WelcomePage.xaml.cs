using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class WelcomePage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;

    public WelcomePage(IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel
        ){
       
        InitializeComponent();
        _uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        this.authenticationServiceMobile = authenticationServiceMobile;
       

    }

    private async void CrtAccount_OnClicked(object? sender, EventArgs e){
        await Navigation.PushAsync(new AppUserRegisterPage(_uiService, authenticationServiceMobile,_userRegisterViewModel));
    }

    private async void Login_OnClicked(object? sender, EventArgs e){
        var loginViewModel = new LoginViewModel(_uiService, authenticationServiceMobile, _userRegisterViewModel)
        {
            UserName = "",
            Password = ""
        };

        var loginPage = new LoginPage(loginViewModel, _uiService, authenticationServiceMobile, _userRegisterViewModel)
        {
            BindingContext = loginViewModel
        };

        await Navigation.PushAsync(loginPage);
    }
}