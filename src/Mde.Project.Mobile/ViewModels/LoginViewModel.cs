using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Services;

using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly IUiService uiService;
   // private readonly IAppUserService _appUserService;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
   

    public ICommand LoginCommand { get; }
    public ICommand FaceLoginCommand { get; }

    public LoginViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel, INativeAuthentication nativeAuthentication 
   // IAppUserService appUserService
    )
    {
       
        this.uiService = uiService;
        authenticationServiceMobile = authServiceMobile;
        _userRegisterViewModel = userRegisterViewModel;
        _nativeAuthentication = nativeAuthentication;
       // _appUserService = appUserService;
        LoginCommand = new RelayCommand(async () => await ExecuteLoginCommand());
        FaceLoginCommand = new RelayCommand(async () => await ExecuteFaceLoginCommand());
        
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
    
    private string fcm;
    public string Fcm
    {
        get => fcm;
        set => SetProperty(ref fcm, value);
    }
 

    public async Task<bool> ExecuteLoginCommand()
    {
        // Valideer invoer: gebruikersnaam en wachtwoord
        if (string.IsNullOrWhiteSpace(UserName))
        {
            await uiService.ShowSnackbarWarning("Username cannot be empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            await uiService.ShowSnackbarWarning("Password cannot be empty.");
            return false;
        }

        // Controleer of het e-mailadres geldig is
        if (!IsValidEmail(UserName))
        {
            await uiService.ShowSnackbarWarning("Please enter a valid email address.");
            return false;
        }

        // Probeer in te loggen met de gevalideerde gegevens
        var isAuthenticated = await authenticationServiceMobile.TryLoginAsync(UserName, Password);
        if (isAuthenticated)
        {
            // Bij succes, navigeer naar de hoofdpagina
 
 

            Application.Current.MainPage = new AppShell(authenticationServiceMobile, uiService, _userRegisterViewModel, this, _nativeAuthentication);
            await Shell.Current.GoToAsync("//CargoListPage");
            return true;
        }
        else
        {
            // Bij mislukking, toon foutmelding
            await uiService.ShowSnackbarWarning("Login Failed. Please check your username and password and try again.");
            return false;
        }
    }

    
    public async Task<bool> ExecuteFaceLoginCommand()
    {
       
        var isAuthenticated = new NativeAuthResult();
        try
        {
#if __ANDROID__
            isAuthenticated = await _nativeAuthentication.PromptLoginAsync("Please authenticate to proceed");
#elif __IOS__
        isAuthenticated = await _nativeAuthentication.PromptLoginAsync("Please authenticate to proceed");
#endif

            if (isAuthenticated.Authenticated)
            {
                var username = "s@t.com";  // Verondersteld dat deze informatie veilig wordt opgeslagen of opgehaald
                var password = "1234";  // Verondersteld dat deze veilig wordt behandeld
               
                var loginResult = await authenticationServiceMobile.TryLoginAsync(username, password);
                if (loginResult)
                {
                   
                    Application.Current.MainPage = new AppShell(authenticationServiceMobile, uiService, _userRegisterViewModel, this, _nativeAuthentication);
                    await Shell.Current.GoToAsync("//CargoListPage");
                    return true;
                }
                else
                {
                    await uiService.ShowSnackbarWarning("Automatic login failed after biometric authentication.");
                }
            }
            else
            {
                await uiService.ShowSnackbarWarning($"Authentication  failed. Error: {isAuthenticated.ErrorMessage}. Please try again.");
            }
        }
        catch (Exception ex)
        {
            await uiService.ShowSnackbarWarning($"An error occurred during biometric authentication: {ex.Message}");
        }
        return false;
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

  
    
   