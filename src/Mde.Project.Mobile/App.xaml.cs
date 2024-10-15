using System.Diagnostics;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Pages;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile;
public partial class App : Application
{
    private readonly IAuthenticationServiceMobile _authenticationService;
    private readonly IWebCargoService _cargoService;
    
    private readonly IUiService uiService;
    public App(IAuthenticationServiceMobile authenticationService, IUiService uiService, IWebCargoService cargoService)
    {
        InitializeComponent();
        _authenticationService = authenticationService;
        _cargoService = cargoService;
        this.uiService = uiService;
        MainPage = new WelcomePage(uiService, authenticationService);
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

            MainPage = new CargoListPage(new CargoListViewModel(_cargoService,uiService)); 
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            MainPage = new NavigationPage(new WelcomePage(uiService, _authenticationService));
        }
    }
}
