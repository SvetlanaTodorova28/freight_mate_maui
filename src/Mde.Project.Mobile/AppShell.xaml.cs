
using Mde.Project.Mobile.Domain.Services.Interfaces;

using Mde.Project.Mobile.ViewModels;


namespace Mde.Project.Mobile
{
    public partial class AppShell : Shell
    {
        
        private readonly IAuthenticationServiceMobile _authenticationService;
        private readonly INativeAuthentication _nativeAuthentication;
        private readonly IUiService _uiService;
        private readonly IAppUserService _appUserService;
        private readonly AppUserRegisterViewModel _userRegisterViewModel;
        private readonly LoginViewModel _loginViewModel;
        public AppShell( IAuthenticationServiceMobile authenticationService, IUiService uiService,
        AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel,
        INativeAuthentication nativeAuthentication)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userRegisterViewModel = userRegisterViewModel;
            _loginViewModel = loginViewModel;
            _nativeAuthentication = nativeAuthentication;
            _uiService = uiService;
            BindingContext = new BaseViewModel(_uiService, _authenticationService, _userRegisterViewModel, _loginViewModel, _nativeAuthentication,
                _appUserService);
          
            
        }
        private async void SiteLink_OnClicked(object? sender, EventArgs e){
            await Launcher.OpenAsync("https://bold.pro/my/todorova-svetlana/310r");
        }
        
       
        
    }
}
