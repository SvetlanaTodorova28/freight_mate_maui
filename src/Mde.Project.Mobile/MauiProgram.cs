using CommunityToolkit.Maui;
using Mde.Project.Mobile.Domain;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.Pages;
using Mde.Project.Mobile.Platforms;
using Mde.Project.Mobile.ViewModels;
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

            RegisterRoutes();
            RegisterServices(builder.Services);

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

            var app = builder.Build();

            _ = Task.Run(async () =>
            {
                await StartupHelper.InitializeKeyVaultAsync(app.Services);
                var appUserService = app.Services.GetService<IAppUserService>();
                if (appUserService != null)
                {
                    var firebaseTokenResult = await FirebaseHelper.RetrieveAndStoreFirebaseTokenAsync(appUserService);

                    if (!firebaseTokenResult.IsSuccess)
                    {
                        var uiService = app.Services.GetService<IUiService>();
                        if (uiService != null)
                        {
                            await MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                uiService.ShowSnackbarWarning($"FCM Token Error: {firebaseTokenResult.ErrorMessage}");
                            });
                        }
                    }
                }
            });

            return app;
        }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(CargoCreatePage), typeof(CargoCreatePage));
            Routing.RegisterRoute(nameof(CargoDetailsPage), typeof(CargoDetailsPage));
            Routing.RegisterRoute(nameof(AppUserRegisterPage), typeof(AppUserRegisterPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(TranslatePage), typeof(TranslatePage));
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<WelcomePage>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginPage>();

            services.AddTransient<CargoListPage>();
            services.AddTransient<CargoListViewModel>();

            services.AddTransient<CargoCreatePage>();
            services.AddTransient<CargoCreateViewModel>();

            services.AddTransient<CargoDetailsPage>();
            services.AddTransient<CargoDetailsViewModel>();

            services.AddTransient<AppUserRegisterPage>();
            services.AddTransient<AppUserRegisterViewModel>();

            services.AddTransient<TranslatePage>();
            services.AddTransient<TranslateViewModel>();

            services.AddTransient<IAuthenticationServiceMobile, SecureWebAuthenticationStorage>();
            services.AddTransient<IFunctionAccessService, FunctionAccessService>();
            services.AddTransient<INativeAuthentication, NativeAuthentication>();

            services.AddTransient<IAppUserService, AppUserService>();
            services.AddTransient<ICargoService, CargoService>();
            services.AddTransient<IUiService, UiService>();
            services.AddTransient<ITranslationStorageService, TranslationStorageService>();

            services.AddSingleton<KeyVaultHelper>();
            services.AddSingleton<ISpeechService>(new AzureSpeechService(GlobalConstants.Region));
            services.AddSingleton<ITextToSpeechService>(new AzureTextToSpeechService(GlobalConstants.Region));
            services.AddHttpClient<ITranslationService, AzureTranslationService>(client =>
            {
                client.BaseAddress = new Uri(GlobalConstants.EndPointTranslate);
            });
            services.AddHttpClient<IOcrService, AzureOcrService>(client =>
            {
                client.BaseAddress = new Uri(GlobalConstants.EndPointOCR);
            });
        }
        
      




    }
}
