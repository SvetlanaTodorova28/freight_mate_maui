
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class WelcomePage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    
    private readonly LoginViewModel _loginViewModel; 
    private readonly INativeAuthentication _nativeAuthentication;
    

    public WelcomePage(IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel, INativeAuthentication nativeAuthentication
        ){
       
        InitializeComponent();
        _uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        _loginViewModel = loginViewModel;
        _nativeAuthentication = nativeAuthentication;
        this.authenticationServiceMobile = authenticationServiceMobile;
       

    }

    private async void CrtAccount_OnClicked(object? sender, EventArgs e){
        await Navigation.PushAsync(new AppUserRegisterPage(_uiService, authenticationServiceMobile,_userRegisterViewModel, _loginViewModel,_nativeAuthentication ));
    }

    private async void Login_OnClicked(object? sender, EventArgs e){
        var loginViewModel = new LoginViewModel(_uiService, authenticationServiceMobile, _userRegisterViewModel,
            _nativeAuthentication)
        {
            UserName = "",
            Password = ""
        };

        var loginPage = new LoginPage(loginViewModel, _uiService, authenticationServiceMobile, _userRegisterViewModel, _nativeAuthentication)
        {
            BindingContext = loginViewModel
        };

        await Navigation.PushAsync(loginPage);
    }
}