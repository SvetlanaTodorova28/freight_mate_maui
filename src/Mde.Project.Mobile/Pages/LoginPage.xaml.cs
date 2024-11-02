
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using Plugin.Maui.Biometric;

namespace Mde.Project.Mobile.Pages;

public partial class LoginPage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;
    private readonly IAuthFaceRecognition _authFaceRecognition;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly LoginViewModel _loginViewModel;

    private readonly
        ICargoService _cargoService;

    public LoginPage(LoginViewModel loginViewModel, IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
    AppUserRegisterViewModel userRegisterViewModel, IAuthFaceRecognition authFaceRecognition){

        InitializeComponent();
        BindingContext = _loginViewModel = loginViewModel;
        _uiService = uiService;
        _userRegisterViewModel = userRegisterViewModel;
        _authenticationServiceMobile = authenticationServiceMobile; 
        _authFaceRecognition = authFaceRecognition;
       
    }
    

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync(new WelcomePage(_uiService, _authenticationServiceMobile, _userRegisterViewModel,
            _loginViewModel, _authFaceRecognition ));
    }

    private async void Login_OnClicked(object? sender, EventArgs e){
        await Navigation.PushModalAsync(new LoadingPageBase());
       
        bool isLoggedIn = await _loginViewModel.ExecuteLoginCommand();
        
        await Navigation.PopModalAsync();

        if (isLoggedIn){
            _uiService.ShowSnackbarSuccessAsync("You are succefully logged in");
            await Task.Delay(8000);
           
           
        }

    }

    private async void LoginWithFace_OnClicked(object? sender, EventArgs e){
        await Navigation.PushModalAsync(new LoadingPageBase());
       
        bool isLoggedIn = await _loginViewModel.ExecuteFaceLoginCommand();
        
        await Navigation.PopModalAsync();

        if (isLoggedIn){
            _uiService.ShowSnackbarSuccessAsync("You are succefully logged in");
            await Task.Delay(8000);
            
           
        }
    }
}