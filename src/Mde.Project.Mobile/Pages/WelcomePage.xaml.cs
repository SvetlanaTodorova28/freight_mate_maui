using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class WelcomePage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    public WelcomePage(IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile){
       
        InitializeComponent();
        _uiService = uiService;
        this.authenticationServiceMobile = authenticationServiceMobile;
       

    }

    private async void CrtAccount_OnClicked(object? sender, EventArgs e){
        await Navigation.PushAsync(new AppUserRegisterPage(_uiService, authenticationServiceMobile));
    }

    private async void Login_OnClicked(object? sender, EventArgs e){
        var loginViewModel = new LoginViewModel(_uiService, authenticationServiceMobile)
        {
            UserName = "",
            Password = ""
        };

        var loginPage = new LoginPage(loginViewModel, _uiService, authenticationServiceMobile)
        {
            BindingContext = loginViewModel
        };

        await Navigation.PushAsync(loginPage);
    }
}