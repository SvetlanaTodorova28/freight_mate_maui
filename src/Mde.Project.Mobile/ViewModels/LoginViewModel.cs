using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;

namespace Mde.Project.Mobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly IUiService _uiService;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly IAppUserService _appUserService;

    public ICommand LoginCommand { get; }
    public ICommand FaceLoginCommand { get; }

    public LoginViewModel(
        IUiService uiService,
        IAuthenticationServiceMobile authenticationServiceMobile,
        AppUserRegisterViewModel userRegisterViewModel,
        INativeAuthentication nativeAuthentication,
        IAppUserService appUserService)
    {
        _uiService = uiService;
        _authenticationServiceMobile = authenticationServiceMobile;
        _userRegisterViewModel = userRegisterViewModel;
        _nativeAuthentication = nativeAuthentication;
        _appUserService = appUserService;

        LoginCommand = new AsyncRelayCommand(ExecuteLoginCommandAsync);
        FaceLoginCommand = new AsyncRelayCommand(ExecuteFaceLoginCommandAsync);
    }

    private string _username;
    public string UserName
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _fcm;
    public string Fcm
    {
        get => _fcm;
        set => SetProperty(ref _fcm, value);
    }

    public async Task ExecuteLoginCommandAsync()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            await _uiService.ShowSnackbarWarning("Username cannot be empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            await _uiService.ShowSnackbarWarning("Password cannot be empty.");
            return;
        }

        if (!EmailValidator.IsValidEmail(UserName))
        {
            await _uiService.ShowSnackbarWarning("Please enter a valid email address.");
            return;
        }

        var loginResult = await _authenticationServiceMobile.TryLoginAsync(UserName, Password);

        if (loginResult.IsSuccess)
        { 
            
            var fcmResult = await FirebaseHelper.UpdateFcmTokenOnServerAsync(_appUserService);
            if (!fcmResult.IsSuccess)
            {
                await _uiService.ShowSnackbarWarning($"Failed to update FCM token: {fcmResult.ErrorMessage}");
            }
            Application.Current.MainPage = new AppShell(_authenticationServiceMobile, _uiService, _userRegisterViewModel, this, _nativeAuthentication, _appUserService);
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            await _uiService.ShowSnackbarWarning(loginResult.ErrorMessage ?? "Login Failed. Please check your username and password and try again.");
        }
    }

    public async Task ExecuteFaceLoginCommandAsync()
    {
        NativeAuthResult isAuthenticated;
        try
        {
#if __ANDROID__
            isAuthenticated = await _nativeAuthentication.PromptLoginAsync("Please authenticate to proceed");
#elif __IOS__
            isAuthenticated = await _nativeAuthentication.PromptLoginAsync("Please authenticate to proceed");
#else
            await _uiService.ShowSnackbarWarning("Biometric authentication is not supported on this platform.");
            return;
#endif

            if (isAuthenticated.Authenticated)
            {
                string username;
                string password;
#if __ANDROID__
                username = "milka@speedy.gr";  
                password = "Advanced1234"; 
#elif __IOS__
                username = "s@t.com";  
                password = "1234"; 
#endif
                var loginResult = await _authenticationServiceMobile.TryLoginAsync(username, password);
                if (loginResult.IsSuccess)
                {
                    var fcmResult = await FirebaseHelper.UpdateFcmTokenOnServerAsync(_appUserService);
                    if (!fcmResult.IsSuccess)
                    {
                        await _uiService.ShowSnackbarWarning($"Failed to update FCM token: {fcmResult.ErrorMessage}");
                    }
                    Application.Current.MainPage = new AppShell(_authenticationServiceMobile, _uiService, _userRegisterViewModel, this, _nativeAuthentication, _appUserService);
                    await Shell.Current.GoToAsync("//CargoListPage");
                }
                else
                {
                    await _uiService.ShowSnackbarWarning(loginResult.ErrorMessage ?? "Automatic login failed after biometric authentication.");
                }
            }
            else
            {
                await _uiService.ShowSnackbarWarning(isAuthenticated.ErrorMessage ?? "Authentication failed. Please try again.");
            }
        }
        catch (Exception ex)
        {
            await _uiService.ShowSnackbarWarning($"An error occurred during biometric authentication: {ex.Message}");
        }
    }

   
}
