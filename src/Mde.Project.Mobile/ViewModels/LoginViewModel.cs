using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly IUiService uiService;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;

    public ICommand LoginCommand { get; }

    public LoginViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel)
    {
       
        this.uiService = uiService;
        authenticationServiceMobile = authServiceMobile;
        _userRegisterViewModel = userRegisterViewModel;
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
        
        if (string.IsNullOrWhiteSpace(UserName))
        {
            await uiService.ShowSnackbarWarning("Username cannot be empty.");
            return;
        }
    
        if (string.IsNullOrWhiteSpace(Password))
        {
            await uiService.ShowSnackbarWarning("Password cannot be empty.");
            return;
        }

        
        if (!IsValidEmail(UserName))
        {
            await uiService.ShowSnackbarWarning("Please enter a valid email address.");
            return;
        }

        var isAuthenticated = await authenticationServiceMobile.TryLoginAsync(UserName, Password);
        if (isAuthenticated)
        {
            
            Application.Current.MainPage = new AppShell(authenticationServiceMobile, uiService, _userRegisterViewModel, this);
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            await uiService.ShowSnackbarWarning("Login Failed. Please check your username and password and try again.");
        }
    }


    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

}

  
    
   