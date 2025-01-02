using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.OS;
using AndroidX.Core.App;
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
            CreateNotificationChannel();

            if (CheckSelfPermission(Manifest.Permission.RecordAudio) != Permission.Granted)
            {
                RequestPermissions(Permissions, RequestMicrophonePermissionId);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Platform.Init(this, savedInstanceState);
            }
            var userService = MauiProgram.CreateMauiApp().Services.GetService<IAppUserService>() as AppUserService;
            if (userService != null){
                FirebaseHelper.RetrieveAndStoreFcmTokenLocallyAsync();
            }

        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string channelName = "General Notifications";
                string channelId = "default_channel";
                string channelDescription = "General notifications";
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
            var channelId = "default_channel";

            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetContentTitle("New Message Received")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetVisibility(NotificationCompat.VisibilityPublic);


            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
     
    }
    
}
