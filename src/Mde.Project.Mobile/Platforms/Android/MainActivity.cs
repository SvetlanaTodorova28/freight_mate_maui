using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.OS;
using AndroidX.Core.App;
using Google.Apis.Auth.OAuth2;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.ViewModels;
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
       

        private CargoListViewModel _cargoListViewModel =
            MauiProgram.CreateMauiApp().Services.GetService<CargoListViewModel>();
        readonly string[] Permissions = { 
            Manifest.Permission.RecordAudio, 
            Manifest.Permission.Camera,
            Manifest.Permission.AccessFineLocation, 
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.ReadMediaImages,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.ReadExternalStorage
        };

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CreateNotificationChannel();
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Platform.Init(this, savedInstanceState);
            }

        }
        
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            CheckForRefresh(intent);
        }

      

        private void CheckForRefresh(Intent intent)
        {
            if (intent.Extras != null && intent.HasExtra("refreshList"))
            {
                bool shouldRefresh = intent.GetBooleanExtra("refreshList", false);
                if (shouldRefresh)
                {
                    _cargoListViewModel?.RefreshListAsync();
                }
            }
        }
        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string channelName = "Firebase Notifications";
                string channelId = "firebase_notifications";
                string channelDescription = "Firebase Notifications";
                NotificationImportance importance = NotificationImportance.High;

                NotificationChannel channel = new NotificationChannel(channelId, channelName, importance)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        protected override void OnResume(){
            base.OnResume();
           // MessagingCenter.Send(this, "CargoListUpdatedRemotely", true);
           _cargoListViewModel?.RefreshListAsync();
        }
     
    }
    
}
