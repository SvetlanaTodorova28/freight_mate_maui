
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using SkiaSharp;

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

    private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;

        canvas.Clear();

        using (var paint = new SKPaint())
        {
            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.White;

            var rect = new SKRect(0, 0, e.Info.Width, 100);
            var path = new SKPath();
            path.MoveTo(0, 100);
            path.QuadTo(e.Info.Width / 2, 0, e.Info.Width, 100);
            path.LineTo(e.Info.Width, 0);
            path.LineTo(0, 0);
            path.Close();

            canvas.DrawPath(path, paint);
        }
    }

    
}