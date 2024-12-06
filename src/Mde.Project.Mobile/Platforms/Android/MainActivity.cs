using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;


using Color = Android.Graphics.Color;

namespace Mde.Project.Mobile.Platforms;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    const int RequestMicrophonePermissionId = 10;
    readonly string[] Permissions = { Android.Manifest.Permission.RecordAudio };

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        CreateNotificationChannel();
        if (CheckSelfPermission(Android.Manifest.Permission.RecordAudio) != Permission.Granted)
        {
            RequestPermissions(Permissions, RequestMicrophonePermissionId);
        }

        
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            Platform.Init(this, savedInstanceState); 
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
                Description = channelDescription,
                LightColor = Color.Beige
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
