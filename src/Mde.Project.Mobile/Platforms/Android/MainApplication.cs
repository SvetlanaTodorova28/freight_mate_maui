using Android.App;
using Android.Gms.Tasks;
using Android.Runtime;
using Firebase;
using Firebase.Messaging;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Platforms;

[Application]
public class MainApplication : MauiApplication{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership){
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    public override void OnCreate()
    {
        base.OnCreate();

        // Initialize Firebase for Android
        FirebaseApp.InitializeApp(this);
        FirebaseMessaging.Instance.AutoInitEnabled = true;
        RetrieveFirebaseToken();
        
       
    }
    private void RetrieveFirebaseToken()
    {
        FirebaseMessaging.Instance.GetToken().AddOnSuccessListener(new OnTokenSuccessListener());
    }
   
}
public class OnTokenSuccessListener : Java.Lang.Object, IOnSuccessListener
{
    public void OnSuccess(Java.Lang.Object result)
    {
        string fcmToken = result.ToString();
        Console.WriteLine($"Firebase registration token: {fcmToken}");

       
        var authService = MauiProgram.CreateMauiApp().Services.GetService<IAuthenticationServiceMobile>() as SecureWebAuthenticationStorage;
        authService?.StoreFcmToken(fcmToken);
    }
}
