
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class WelcomePage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly IAppUserService _appUserService;
    private readonly LoginViewModel _loginViewModel; 
    private readonly IMainThreadInvoker _mainThreadInvoker; 
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly IFirebaseTokenService _firebaseTokenService;
    

    public WelcomePage(IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel, INativeAuthentication nativeAuthentication,
    IAppUserService appUserService, IMainThreadInvoker mainThreadInvoker, IFirebaseTokenService firebaseTokenService
    ){
       
        InitializeComponent();
        _uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        _loginViewModel = loginViewModel;
        _nativeAuthentication = nativeAuthentication;
        _authenticationServiceMobile = authenticationServiceMobile;
        _appUserService = appUserService;
       _mainThreadInvoker = mainThreadInvoker;
        _firebaseTokenService = firebaseTokenService;

    }

    private async void CrtAccount_OnClicked(object? sender, EventArgs e){
        await Navigation.PushAsync(new AppUserRegisterPage(_uiService, _authenticationServiceMobile,_userRegisterViewModel, _loginViewModel,_nativeAuthentication,
            _appUserService, _mainThreadInvoker, _firebaseTokenService));
    }

    private async void Login_OnClicked(object? sender, EventArgs e){
        var loginViewModel = new LoginViewModel(_uiService, _authenticationServiceMobile, _userRegisterViewModel,
            _nativeAuthentication, _appUserService, _mainThreadInvoker, _firebaseTokenService) 
        {
            UserName = "",
            Password = ""
        };

        var loginPage = new LoginPage(loginViewModel, 
            _uiService,
            _authenticationServiceMobile, 
            _userRegisterViewModel, 
            _nativeAuthentication,
            _appUserService,
            _mainThreadInvoker,
            _firebaseTokenService)
        {
            BindingContext = loginViewModel
        };

        await Navigation.PushAsync(loginPage);
    }
}