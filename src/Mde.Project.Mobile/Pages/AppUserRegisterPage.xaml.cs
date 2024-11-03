
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class AppUserRegisterPage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly LoginViewModel _loginViewModel;
    
   
    public AppUserRegisterPage(IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
        AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel, INativeAuthentication nativeAuthentication){
        InitializeComponent();
        _uiService = uiService;
        _loginViewModel = loginViewModel;
        _nativeAuthentication = nativeAuthentication;
         BindingContext =  _userRegisterViewModel = userRegisterViewModel;
        this.authenticationServiceMobile = authenticationServiceMobile;
       
    }

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync( new WelcomePage(_uiService, authenticationServiceMobile,_userRegisterViewModel, _loginViewModel, _nativeAuthentication));
    }
    
    protected override void OnAppearing()
    {
        _userRegisterViewModel?.OnAppearingCommand.Execute(null);
    }

    private async void CreateAccount_OnClicked(object sender, EventArgs e){

        await Navigation.PushModalAsync(new LoadingPage());
       
        bool isRegistered = await _userRegisterViewModel.ExecuteRegisterCommand();
        
        await Navigation.PopModalAsync();

        if (isRegistered){
            _uiService.ShowSnackbarSuccessAsync("Your account is successfully created. You can login now");
            Application.Current.MainPage = new NavigationPage(new LoginPage(_loginViewModel,_uiService, 
                authenticationServiceMobile,_userRegisterViewModel, _nativeAuthentication));
        }
       
    }

    
    
}