using Mde.Project.Mobile.Pages;
using Microsoft.Extensions.Logging;

namespace Mde.Project.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fontawesome-webfont.ttf", "FontAwesome");
                    fonts.AddFont("FontAwesomeSolid.otf", "AwesomeSolid");
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
