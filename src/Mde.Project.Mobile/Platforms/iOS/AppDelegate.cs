
using Foundation;
using UIKit;
using UserNotifications;
using Firebase.CloudMessaging;
using Mde.Project.Mobile.Helpers;

namespace Mde.Project.Mobile.Platforms;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IMessagingDelegate, IUNUserNotificationCenterDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        Firebase.Core.App.Configure();
        RegisterForRemoteNotifications(app);
        Messaging.SharedInstance.Delegate = this;
        UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();
        return base.FinishedLaunching(app, options);
    }
    void RegisterForRemoteNotifications(UIApplication app)
    {
        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
            (granted, error) =>
            {
                if (granted)
                    InvokeOnMainThread(() => app.RegisterForRemoteNotifications());
            });
    }

    [Export("messaging:didReceiveRegistrationToken:")]
    public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
    {
        var userService = MauiProgram.CreateMauiApp().Services.GetService<IAppUserService>() as AppUserService;
        if (userService != null){
            FirebaseHelper.RetrieveAndStoreFcmTokenLocallyAsync();
        }
    }
    
}

public class NotificationDelegate : UNUserNotificationCenterDelegate
{
    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
        MessagingCenter.Send<NotificationDelegate, bool>(this, "CargoListUpdatedRemotely", true);
    }
}
