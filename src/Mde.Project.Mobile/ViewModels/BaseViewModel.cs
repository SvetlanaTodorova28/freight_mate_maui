using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class BaseViewModel:ObservableObject
{
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly IUiService _uiService;
    private readonly IAppUserService _appUserService;
    private readonly IMainThreadInvoker _mainThreadInvoker;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly LoginViewModel _loginViewModel;
    
    public BaseViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel, INativeAuthentication serviceNativeAuthentication,
    IAppUserService appUserService, IMainThreadInvoker mainThreadInvoker)
    {
       
        _uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        _loginViewModel = loginViewModel;
        _authenticationServiceMobile = authServiceMobile;
        _nativeAuthentication = serviceNativeAuthentication;
        _appUserService = appUserService;
        _mainThreadInvoker = mainThreadInvoker;
    }
    
    public ICommand OnLogoutCommand => new Command(async () => await LogoutAsync());

    private async Task LogoutAsync()
    {
        
        var result = await _authenticationServiceMobile.Logout();
        
        if (result.IsSuccess && result.Data)
        {
           
            
            Application.Current.MainPage = new NavigationPage(new WelcomePage(_uiService, _authenticationServiceMobile,
                _userRegisterViewModel, _loginViewModel, _nativeAuthentication, _appUserService, _mainThreadInvoker)); 
          
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Fout", result.ErrorMessage ?? "Logout mislukt", "OK");
        }
    }


}