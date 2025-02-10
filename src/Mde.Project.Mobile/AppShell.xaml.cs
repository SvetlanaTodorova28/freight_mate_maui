
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Pages;
using Mde.Project.Mobile.ViewModels;


namespace Mde.Project.Mobile
{
    public partial class AppShell : Shell
    {
        
        private readonly IAuthenticationServiceMobile _authenticationService;
        private readonly INativeAuthentication _nativeAuthentication;
        private readonly IUiService _uiService;
        private readonly IAppUserService _appUserService;
        private readonly IMainThreadInvoker _mainThreadInvoker;
        private readonly AppUserRegisterViewModel _userRegisterViewModel;
        private readonly LoginViewModel _loginViewModel;
        private readonly IFirebaseTokenService _firebaseTokenService;
        public AppShell( IAuthenticationServiceMobile authenticationService, IUiService uiService,
        AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel,
        INativeAuthentication nativeAuthentication, IAppUserService appUserService, IMainThreadInvoker mainThreadInvoker,
        IFirebaseTokenService firebaseTokenService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userRegisterViewModel = userRegisterViewModel;
            _loginViewModel = loginViewModel;
            _nativeAuthentication = nativeAuthentication;
            _uiService = uiService;
            _appUserService = appUserService;
            _mainThreadInvoker = mainThreadInvoker;
            _firebaseTokenService = firebaseTokenService;
            BindingContext = new BaseViewModel(_uiService, _authenticationService, _userRegisterViewModel, _loginViewModel, _nativeAuthentication,
                _appUserService, _mainThreadInvoker, _firebaseTokenService);
          
            
        }
        private async void SiteLink_OnClicked(object? sender, EventArgs e){
            await Launcher.OpenAsync("https://bold.pro/my/todorova-svetlana/310r");
        }


// Example of navigating to WebViewLinkdInPage with a URL parameter
        private async void LinkLinkdIn_OnClicked(Object sender, EventArgs e)
        {
            string linkedInUrl = "https://www.linkedin.com/in/svetlana-todorova-87354bbb";
            await Current.GoToAsync($"{nameof(WebViewLinkdInPage)}?url={linkedInUrl}");
        }

    }
}
