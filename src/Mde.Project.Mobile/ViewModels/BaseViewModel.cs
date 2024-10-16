using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class BaseViewModel:ObservableObject
{
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly IUiService uiService;

    
    public BaseViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile)
    {
       
        this.uiService = uiService;
        authenticationServiceMobile = authServiceMobile;
        
    }
    
    public ICommand OnLogoutCommand => new Command(async () => await LogoutAsync());

    private async Task LogoutAsync()
    {
        bool success = authenticationServiceMobile.Logout();

        if(success)
        {
            Application.Current.MainPage = new NavigationPage(new WelcomePage(uiService, authenticationServiceMobile));
        }
    }
    
}