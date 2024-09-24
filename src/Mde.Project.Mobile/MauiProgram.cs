using CommunityToolkit.Maui;
using Mde.Project.Mobile.Pages;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;


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
            Routing.RegisterRoute("//pages/createAccount", typeof(CreateAccountPage));
            Routing.RegisterRoute("//pages/about", typeof(AboutPage));
            Routing.RegisterRoute("//pages/createCargo", typeof(CreateCargoPage));
            Routing.RegisterRoute("//pages/infoCargo", typeof(CargoInfoPage));

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
