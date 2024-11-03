
using Mde.Project.Mobile.Domain.Services.Interfaces;

using Mde.Project.Mobile.ViewModels;


namespace Mde.Project.Mobile
{
    public partial class AppShell : Shell
    {
        
        private readonly IAuthenticationServiceMobile _authenticationService;
        private readonly INativeAuthentication _nativeAuthentication;
        private readonly IUiService uiService;
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
            this.uiService = uiService;
            BindingContext = new BaseViewModel(uiService, _authenticationService, _userRegisterViewModel, _loginViewModel, _nativeAuthentication);
          
            
        }
        private async void SiteLink_OnClicked(object? sender, EventArgs e){
            await Launcher.OpenAsync("https://bold.pro/my/todorova-svetlana/310r");
        }
        
       
        
    }
}
