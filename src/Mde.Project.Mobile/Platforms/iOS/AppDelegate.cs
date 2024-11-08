
using Foundation;
using UIKit;
using UserNotifications;
using Firebase.CloudMessaging;
using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;


namespace Mde.Project.Mobile.Platforms;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IMessagingDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        Firebase.Core.App.Configure();
        Console.WriteLine("Firebase configured");

        Messaging.SharedInstance.Delegate = this;

        // Vraag toestemming voor meldingen
        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
        {
            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                (granted, error) =>
                {
                    Console.WriteLine($"Notification permission granted: {granted}");
                    if (granted)
                        InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                });
            UNUserNotificationCenter.Current.Delegate = new NotificationDelegate();
        }

        return base.FinishedLaunching(app, options);
    }


    
    [Export("messaging:didReceiveRegistrationToken:")]
    public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken){

        var userService = MauiProgram.CreateMauiApp().Services.GetService<IAppUserService>() as AppUserService;
       userService?.StoreFcmTokenAsync(fcmToken);
    }



    public  void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
    {
        
        Messaging.SharedInstance.ApnsToken = deviceToken;
    }
}

public class NotificationDelegate : UNUserNotificationCenterDelegate
{
    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        
        completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
    }
}
