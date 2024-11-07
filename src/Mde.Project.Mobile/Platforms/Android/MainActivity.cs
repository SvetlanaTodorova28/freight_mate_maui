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
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        CreateNotificationChannel();
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
            .SetSmallIcon(Resource.Drawable.cargos)  // Zorg dat deze resource bestaat in je Drawable map
            .SetContentTitle("Nieuwe Melding")
            .SetContentText(messageBody)
            .SetAutoCancel(true)
            .SetVisibility(NotificationCompat.VisibilityPublic); // Voor heads-up notificatie

        var notificationManager = NotificationManagerCompat.From(context);
        notificationManager.Notify(0, notificationBuilder.Build());
    }
}
