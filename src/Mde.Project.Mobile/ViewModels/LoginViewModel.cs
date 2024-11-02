using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly IAuthFaceRecognition _authFaceRecognition;
    private readonly IUiService uiService;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;

    public ICommand LoginCommand { get; }
    public ICommand FaceLoginCommand { get; }

    public LoginViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel, IAuthFaceRecognition authFaceRecognition)
    {
       
        this.uiService = uiService;
        authenticationServiceMobile = authServiceMobile;
        _userRegisterViewModel = userRegisterViewModel;
        _authFaceRecognition = authFaceRecognition;
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

    public async Task<bool> ExecuteLoginCommand()
    {
        
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

        
        if (!IsValidEmail(UserName))
        {
            await uiService.ShowSnackbarWarning("Please enter a valid email address.");
            return false;
        }

        var isAuthenticated = await authenticationServiceMobile.TryLoginAsync(UserName, Password);
        if (isAuthenticated)
        {
            
            Application.Current.MainPage = new AppShell(authenticationServiceMobile, uiService, _userRegisterViewModel, this, _authFaceRecognition);
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            await uiService.ShowSnackbarWarning("Login Failed. Please check your username and password and try again.");
        }
        var isLoggedIn = await authenticationServiceMobile.TryLoginAsync(username, Password);
        return isLoggedIn; 
    }
    
    public async Task<bool> ExecuteFaceLoginCommand()
    {
        // Toon een laadindicator of iets dergelijks om de gebruiker te informeren dat het proces bezig is.
       // uiService.ShowLoading("Authenticating...");

        try
        {
            var isAuthenticated = await _authFaceRecognition.PromptLoginAsync("Please authenticate to proceed");
            // Verberg de laadindicator zodra de authenticatiepoging is voltooid.
            //uiService.HideLoading();
            

            if (isAuthenticated.Authenticated){
                var username = "s@t.com"; // Of laad van Keychain
                var password = "1234"; // Of laad van Keychain
                var loginResult = await authenticationServiceMobile.TryLoginAsync(username, password);
                if (loginResult) {
                    Application.Current.MainPage = new AppShell(authenticationServiceMobile, uiService, _userRegisterViewModel, this, _authFaceRecognition);
                    await Shell.Current.GoToAsync("//CargoListPage");
                } else {
                    await uiService.ShowSnackbarWarning("Automatic login failed.");
                }
            } else {
                await uiService.ShowSnackbarWarning("Face recognition login failed. Please try again.");
            }
        } catch (Exception ex) {
            await uiService.ShowSnackbarWarning("An error occurred: " + ex.Message);
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

  
    
   