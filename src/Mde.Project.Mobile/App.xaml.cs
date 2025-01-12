using System.Diagnostics;
using DotNetEnv;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.Pages;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile;
public partial class App : Application
{
    private readonly IAuthenticationServiceMobile _authenticationService;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly IAppUserService _appUserService;
    private readonly IUiService _uiService;
    private readonly IMainThreadInvoker _mainThreadInvoker;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly LoginViewModel _loginViewModel;
    private readonly IFirebaseTokenService _firebaseTokenService;

    public App(IAuthenticationServiceMobile authenticationService, IUiService uiService, AppUserRegisterViewModel userRegisterViewModel,
        LoginViewModel loginViewModel, INativeAuthentication nativeAuthentication, IAppUserService appUserService, IMainThreadInvoker mainThreadInvoker,
        IFirebaseTokenService firebaseTokenService)
    {
        InitializeComponent();
        _authenticationService = authenticationService;
        _nativeAuthentication = nativeAuthentication;
        _uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        _loginViewModel = loginViewModel;
        _appUserService = appUserService;
        _mainThreadInvoker = mainThreadInvoker;
        _firebaseTokenService = firebaseTokenService;
        MainPage = new WelcomePage(uiService, authenticationService,_userRegisterViewModel,_loginViewModel, _nativeAuthentication, _appUserService, _mainThreadInvoker, _firebaseTokenService);
    }

    protected async override void OnStart()
    {
        base.OnStart();
        await SkipLoginPageIfPossible();
    }

    protected async override void OnResume()
    {
        base.OnResume();
        await SkipLoginPageIfPossible();
    }

    private async Task SkipLoginPageIfPossible()
    {
        var isAuthenticated = await _authenticationService.IsAuthenticatedAsync();
        if (isAuthenticated.IsSuccess)
        {
            MainPage = new AppShell(_authenticationService,_uiService, _userRegisterViewModel, _loginViewModel, _nativeAuthentication, _appUserService,
                _mainThreadInvoker, _firebaseTokenService);
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            MainPage = new NavigationPage(new WelcomePage(_uiService, _authenticationService, _userRegisterViewModel, _loginViewModel, _nativeAuthentication, _appUserService,
                _mainThreadInvoker, _firebaseTokenService));
        }
    }
    
    
   

}
