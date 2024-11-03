using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class BaseViewModel:ObservableObject
{
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly IUiService uiService;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly LoginViewModel _loginViewModel;
    
    public BaseViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel, INativeAuthentication serviceNativeAuthentication)
    {
       
        this.uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        _loginViewModel = loginViewModel;
        authenticationServiceMobile = authServiceMobile;
        _nativeAuthentication = serviceNativeAuthentication;
        
    }
    
    public ICommand OnLogoutCommand => new Command(async () => await LogoutAsync());

    private async Task LogoutAsync()
    {
        bool success = authenticationServiceMobile.Logout();

        if(success)
        {
            Application.Current.MainPage = new NavigationPage(new WelcomePage(uiService, authenticationServiceMobile,
                _userRegisterViewModel, _loginViewModel, _nativeAuthentication));
        }
    }
    
}