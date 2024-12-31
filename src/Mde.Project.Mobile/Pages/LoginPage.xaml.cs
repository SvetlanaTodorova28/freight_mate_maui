using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using SkiaSharp;

namespace Mde.Project.Mobile.Pages;

public partial class LoginPage : ContentPage
{
   
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly IAppUserService _appUserService;
    private readonly LoginViewModel _loginViewModel;
    private readonly IMainThreadInvoker _mainThreadInvoker;
    private bool _isRegistrationSuccessful;

    public LoginPage(LoginViewModel loginViewModel, IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
        AppUserRegisterViewModel userRegisterViewModel,INativeAuthentication nativeAuthentication,
        IAppUserService appUserService, IMainThreadInvoker mainThreadInvoker , bool isRegistrationSuccessful = false)
    {
        _uiService = uiService;
        _loginViewModel = loginViewModel;
        _nativeAuthentication = nativeAuthentication;
        _authenticationServiceMobile = authenticationServiceMobile;
        _uiService = uiService;
        _appUserService = appUserService; 
        _userRegisterViewModel = userRegisterViewModel;
        _mainThreadInvoker = mainThreadInvoker; 
        _isRegistrationSuccessful = isRegistrationSuccessful;
        InitializeComponent();
        BindingContext = _loginViewModel = loginViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_isRegistrationSuccessful)
        {
            _mainThreadInvoker.InvokeOnMainThreadAsync(async () => 
            {
                await _uiService.ShowSnackbarSuccessAsync("Registration successful! You can now login.");
            });
        }
    }

    private void BackToWelcome_OnTapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync( new WelcomePage(_uiService, _authenticationServiceMobile,_userRegisterViewModel, _loginViewModel, _nativeAuthentication,
            _appUserService, _mainThreadInvoker));
    }

    private async void Login_OnClicked(object sender, EventArgs e)
    {
        await ShowLoadingPageAsync(async () =>
        {
            await _loginViewModel.ExecuteLoginCommandAsync();
        });
    }

    private async void LoginWithFace_OnClicked(object sender, EventArgs e)
    {
        await ShowLoadingPageAsync(async () =>
        {
            await _loginViewModel.ExecuteFaceLoginCommandAsync();
        });
    }

    private async Task ShowLoadingPageAsync(Func<Task> action)
    {
        try
        {
            await Navigation.PushModalAsync(new LoadingPageBase());
            await action();
        }
        finally
        {
            await Navigation.PopModalAsync();
        }
    }

    private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear();

        using (var paint = new SKPaint { Style = SKPaintStyle.Fill, IsAntialias = true })
        {
            var colors = new[] { SKColor.Parse("#1D3448"), SKColor.Parse("#35606E"), SKColor.Parse("#1D3448") };
            var colorPositions = new[] { 0.1f, 0.6f, 0.8f };

            paint.Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(e.Info.Width, e.Info.Height),
                colors,
                colorPositions,
                SKShaderTileMode.Clamp);

            var path = CreateCurvedPath(e.Info.Width, e.Info.Height);
            canvas.DrawPath(path, paint);
        }
    }

    private SKPath CreateCurvedPath(float width, float height)
    {
        var path = new SKPath();
        path.MoveTo(width, 0);

        var controlX1 = width - 1.1f * width;
        var controlY1 = 0.1f * height;
        var controlX2 = -0.5f * width;
        var controlY2 = height / 2;

        path.CubicTo(controlX1, controlY1, controlX2, controlY2, width, height);
        path.LineTo(width, 0);
        path.Close();

        return path;
    }
}
