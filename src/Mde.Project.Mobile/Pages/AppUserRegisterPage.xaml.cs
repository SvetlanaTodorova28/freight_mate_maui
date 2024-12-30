
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using SkiaSharp;
using SkiaSharp.Extended;

namespace Mde.Project.Mobile.Pages;

public partial class AppUserRegisterPage : ContentPage{
    private readonly IUiService _uiService;
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;
    private readonly INativeAuthentication _nativeAuthentication;
    private readonly AppUserRegisterViewModel _userRegisterViewModel;
    private readonly IAppUserService _appUserService;
    private readonly LoginViewModel _loginViewModel;
  
   
    public AppUserRegisterPage(IUiService uiService, IAuthenticationServiceMobile authenticationServiceMobile,
        AppUserRegisterViewModel userRegisterViewModel, LoginViewModel loginViewModel, INativeAuthentication nativeAuthentication,
        IAppUserService appUserService){
        InitializeComponent();
        _uiService = uiService;
        _loginViewModel = loginViewModel;
        _nativeAuthentication = nativeAuthentication;
        _authenticationServiceMobile = authenticationServiceMobile;
        _uiService = uiService;
        _appUserService = appUserService; 
         BindingContext =  _userRegisterViewModel = userRegisterViewModel;
      
       
    }

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync( new WelcomePage(_uiService, _authenticationServiceMobile,_userRegisterViewModel, _loginViewModel, _nativeAuthentication,
            _appUserService));
    }
    
    protected override void OnAppearing()
    {
        _userRegisterViewModel?.OnAppearingCommand.Execute(null);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
       // _userRegisterViewModel.ResetRegistrationForm(); 
    }


    private async void CreateAccount_OnClicked(object sender, EventArgs e){

        await Navigation.PushModalAsync(new LoadingPage());
       
        bool isRegistered = await _userRegisterViewModel.ExecuteRegisterCommand();
        
        if (isRegistered){
            Application.Current.MainPage = new NavigationPage(new LoginPage
                (_loginViewModel,
                    _uiService, 
                    _authenticationServiceMobile,
                    _userRegisterViewModel,
                    _nativeAuthentication, 
                    _appUserService));
        }
        else{
            await Navigation.PopModalAsync();
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
           var colors = new [] { SKColor.Parse("#1D3448"), SKColor.Parse("#35606E"), SKColor.Parse("#1D3448") };
           var colorPositions = new float[] { 0.1f, 0.6f, 0.8f };  

           paint.IsAntialias = true;  
           paint.Shader = SKShader.CreateLinearGradient(
               new SKPoint(0, 0),
               new SKPoint(e.Info.Width, e.Info.Height),
               colors,
               colorPositions,
               SKShaderTileMode.Clamp);

           var path = new SKPath();
           
           path.MoveTo(e.Info.Width, 0);

           
           float controlX1 = e.Info.Width - 1.1f * e.Info.Width; 
           float controlY1 = 0.1f * e.Info.Height; 

          
           float controlX2 = -0.5f * e.Info.Width; 
           float controlY2 = e.Info.Height / 2; 

          
           path.CubicTo(controlX1, controlY1, controlX2, controlY2, e.Info.Width, e.Info.Height);
          
           path.LineTo(e.Info.Width, 0);

           path.Close();

           canvas.DrawPath(path, paint);
       }
   }
 
   
}