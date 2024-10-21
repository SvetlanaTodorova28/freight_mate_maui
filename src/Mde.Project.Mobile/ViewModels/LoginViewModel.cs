using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;
namespace Mde.Project.Mobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly IUiService uiService;

    public ICommand LoginCommand { get; }

    public LoginViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile)
    {
       
        this.uiService = uiService;
        authenticationServiceMobile = authServiceMobile;
        LoginCommand = new Command(async () => await ExecuteLoginCommand());
    }

    private string username;
    public string UserName
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    private string password;
    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }

    private async Task ExecuteLoginCommand()
    {
        var isAuthenticated = await authenticationServiceMobile.TryLoginAsync(UserName, Password);
        if (isAuthenticated)
        {
            // Redirect naar de hoofdpagina na succesvolle login
            Application.Current.MainPage = new AppShell(authenticationServiceMobile, uiService);
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            await uiService.ShowSnackbarWarning("Login Failed. Please check your username and password and try again.");
        }
    }
}

  
    
   