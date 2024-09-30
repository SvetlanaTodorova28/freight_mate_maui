using CommunityToolkit.Maui;
using Mde.Project.Mobile.Pages;
using Microsoft.Extensions.Logging;
using Mde.Project.Mobile.ViewModels;



namespace Mde.Project.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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
            Routing.RegisterRoute("//pages/appuserRegister", typeof(AppUserRegisterPage));
            Routing.RegisterRoute("//pages/about", typeof(AboutPage));
            Routing.RegisterRoute("//pages/createCargo", typeof(CargoCreatePage));
            Routing.RegisterRoute("//pages/infoCargo", typeof(CargoInfoPage));
            
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();
            
            builder.Services.AddTransient<CargoListPage>();
            builder.Services.AddTransient<CargoListViewModel>();

            builder.Services.AddTransient<CargoCreatePage>();
            builder.Services.AddTransient<CargoCreateViewModel>();
            
            builder.Services.AddTransient<AppUserRegisterPage>();
            builder.Services.AddTransient<AppUserRegisterViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
