using CommunityToolkit.Maui;
using Mde.Project.Mobile.Domain;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.Pages;
using Mde.Project.Mobile.ViewModels;
using Mde.Project.Mobile.Platforms;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Utilities;


namespace Mde.Project.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
           
            var builder = MauiApp.CreateBuilder();
         
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fontawesome-webfont.ttf", "FontAwesome");
                    fonts.AddFont("FontAwesomeSolid.otf", "AwesomeSolid");
                    fonts.AddFont("Cinzel-VariableFont_wght.ttf", "Cinzel");
                    fonts.AddFont("Play-Bold.ttf", "PlayBold");
                });
           
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(CargoCreatePage), typeof(CargoCreatePage));
            Routing.RegisterRoute(nameof(CargoDetailsPage), typeof(CargoDetailsPage));
            Routing.RegisterRoute(nameof(AppUserRegisterPage), typeof(AppUserRegisterPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(TranslatePage), typeof(TranslatePage));
            
            builder.Services.AddTransient<WelcomePage>();
            
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            
            builder.Services.AddTransient<CargoListPage>();
            builder.Services.AddTransient<CargoListViewModel>();

            builder.Services.AddTransient<CargoCreatePage>();
            builder.Services.AddTransient<CargoCreateViewModel>();
            
            builder.Services.AddTransient<CargoDetailsPage>();
            builder.Services.AddTransient<CargoDetailsViewModel>();
            
            builder.Services.AddTransient<AppUserRegisterPage>();
            builder.Services.AddTransient<AppUserRegisterViewModel>();
            
            builder.Services.AddTransient<TranslatePage>();
            builder.Services.AddTransient<TranslateViewModel>();
            
            builder.Services.AddTransient<IAuthenticationServiceMobile, SecureWebAuthenticationStorage>();
            builder.Services.AddTransient<INativeAuthentication, NativeAuthentication>();
           
            builder.Services.AddTransient<IAppUserService, AppUserService>();
            
            builder.Services.AddTransient<ICargoService, CargoService>();
            builder.Services.AddTransient<IUiService, UiService>();
            builder.Services.AddTransient<ITranslationStorageService, TranslationStorageService>();

            

            builder.Services.AddHttpClient(GlobalConstants.HttpClient, config => config.BaseAddress = new Uri(GlobalConstants.BaseAzure));
            builder.Services.AddHttpClient(GlobalConstants.HttpClientFireBase, config => config.BaseAddress = new Uri(GlobalConstants.BaseUrlFireBase));

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });
            

            
            var keySpeech =  SecureStorageHelper.GetApiKey("Key_Speech");
            var keyTranslation =  SecureStorageHelper.GetApiKey("Key_Translation");
            var keyOCR =  SecureStorageHelper.GetApiKey("Key_OCR");
            string region = "northeurope";
            
            builder.Services.AddSingleton<ISpeechService>(new AzureSpeechService(keySpeech, region));
            builder.Services.AddSingleton<ITextToSpeechService>(new AzureTextToSpeechService(keySpeech, region));
            builder.Services.AddHttpClient<ITranslationService, AzureTranslationService>((client) => {
                client.BaseAddress = new Uri("https://api.cognitive.microsofttranslator.com");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", keyTranslation);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", region);
            });
            
            builder.Services.AddHttpClient<IOcrService, AzureOcrService>(client =>
            {
                client.BaseAddress = new Uri(GlobalConstants.EndPointOCR);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", keyOCR);
            });
            
            return builder.Build();
            
        }

    }
}
