using Foundation;

namespace Mde.Project.Mobile.Platforms;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}