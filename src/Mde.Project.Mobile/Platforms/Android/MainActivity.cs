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
        const int RequestMicrophonePermissionId = 10;

        private CargoListViewModel _cargoListViewModel =
            MauiProgram.CreateMauiApp().Services.GetService<CargoListViewModel>();
        readonly string[] Permissions = { Manifest.Permission.RecordAudio };

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CreateNotificationChannel();


            if (CheckSelfPermission(Manifest.Permission.RecordAudio) != Permission.Granted)
            {
                RequestPermissions(Permissions, RequestMicrophonePermissionId);
            }

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

        public void SendNotification(string messageBody, Context context)
        {
            var channelId = "firebase_notifications";

            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetVisibility(NotificationCompat.VisibilityPublic);

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(0, notificationBuilder.Build());
        }
     
     
    }
    
}
