using Android.App;
using Android.Content.Res;
using Android.Gms.Tasks;
using Android.Runtime;
using Firebase;
using Firebase.Messaging;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Mde.Project.Mobile.Platforms;

[Application]
public class MainApplication : MauiApplication{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership){
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
            if (view is Entry)
            {
                handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            }
        });
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    public override void OnCreate()
    {
        base.OnCreate();
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
    private readonly IUiService _uiService;
    private readonly IAppUserService _userService;
    public OnTokenSuccessListener()
    {
        _uiService = MauiProgram.CreateMauiApp().Services.GetService<IUiService>();
        _userService = MauiProgram.CreateMauiApp().Services.GetService<IAppUserService>() as AppUserService;
    }
    public async void OnSuccess(Java.Lang.Object result)
    {
        try
        {
            string fcmToken = result.ToString();

            if (_userService != null)
            {
                var storeResult = await _userService.StoreFcmTokenAsync(fcmToken);

                if (!storeResult.IsSuccess)
                {
                    await _uiService.ShowSnackbarWarning($"Error storing FCM token: {storeResult.ErrorMessage}");
                }
            }
        }
        catch (Exception ex)
        {
            await _uiService.ShowSnackbarWarning("An unexpected error occurred while storing the FCM token.");
        }
    }
}
