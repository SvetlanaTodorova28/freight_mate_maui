using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.OS;
using AndroidX.Core.App;
using Google.Apis.Auth.OAuth2;
using Mde.Project.Mobile.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Mde.Project.Mobile.Platforms
{
    [Activity(
        Label = "Mde Project Mobile",
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode)]
    public class MainActivity : MauiAppCompatActivity
    {
        const int RequestMicrophonePermissionId = 10;
        readonly string[] Permissions = { Manifest.Permission.RecordAudio };

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
          

            if (CheckSelfPermission(Manifest.Permission.RecordAudio) != Permission.Granted)
            {
                RequestPermissions(Permissions, RequestMicrophonePermissionId);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Platform.Init(this, savedInstanceState);
            }

        }
        
     
    }
    
}
