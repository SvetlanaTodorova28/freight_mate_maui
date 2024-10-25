using System.Diagnostics;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Pages;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile;
public partial class App : Application
{
    private readonly IAuthenticationServiceMobile _authenticationService;
    private readonly ICargoService _cargoService;
    private readonly IUiService uiService;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    public App(IAuthenticationServiceMobile authenticationService, IUiService uiService, ICargoService cargoService,  AppUserRegisterViewModel userRegisterViewModel)
    {
        InitializeComponent();
        _authenticationService = authenticationService;
        _cargoService = cargoService;
        this.uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        MainPage = new WelcomePage(uiService, authenticationService,_userRegisterViewModel);
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
        bool isAuthenticated = await _authenticationService.IsAuthenticatedAsync();
        if (isAuthenticated)
        {
            Debug.WriteLine($"Authentication Service Initialized: {_authenticationService != null}");

            MainPage = new AppShell(_authenticationService,uiService, _userRegisterViewModel);
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            MainPage = new NavigationPage(new WelcomePage(uiService, _authenticationService, _userRegisterViewModel));
        }
    }
    
   

}
