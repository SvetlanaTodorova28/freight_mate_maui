
using Foundation;
using UIKit;
using UserNotifications;
using Firebase.CloudMessaging;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Platforms;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IMessagingDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        // Configure Firebase
        Firebase.Core.App.Configure();

        // Set the Messaging delegate
        Messaging.SharedInstance.Delegate = this;

        // Request Notification Permissions
        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
        {
            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                (granted, error) =>
                {
                    if (granted)
                        InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                });
            UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();
        }
        else
        {
            var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        return base.FinishedLaunching(app, options);
    }

    // Firebase Messaging Delegate methods
    public  void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
    {
        var authService = MauiProgram.CreateMauiApp().Services.GetService<IAuthenticationServiceMobile>() as SecureWebAuthenticationStorage;
        authService?.StoreFcmToken(fcmToken);
    }


    public  void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
    {
        // Map APNs token to FCM token
        Messaging.SharedInstance.ApnsToken = deviceToken;
    }
}

public class NotificationDelegate : UNUserNotificationCenterDelegate
{
    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        // Present the notification while the app is in the foreground
        completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
    }
}
