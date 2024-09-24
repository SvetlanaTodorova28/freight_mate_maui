using Android.App;
using Android.Runtime;
using Mde.Project.Mobile;

namespace pfff;

[Application]
public class MainApplication : MauiApplication{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership){
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}