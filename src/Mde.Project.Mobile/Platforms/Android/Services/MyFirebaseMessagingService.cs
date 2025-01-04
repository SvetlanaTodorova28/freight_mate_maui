using Android.App;
using Firebase.Messaging;

namespace Mde.Project.Mobile.Platforms;
    [Service(Name = "Mde.Project.Mobile.Platforms.MyFirebaseMessagingService", Exported = true)]
    [IntentFilter(new[]{ "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService{
        public override void OnMessageReceived(RemoteMessage message){
            base.OnMessageReceived(message);

            string title = message.GetNotification().Title;
            string body = message.GetNotification().Body;

            if (title != null && body != null){
                SendNotification(title, body);
            }
            
            MessagingCenter.Send<MyFirebaseMessagingService, bool>(this, "CargoListUpdatedRemotely", true);
        }

        void SendNotification(string title, string body){
                var notificationManager = NotificationManager.FromContext(this);

                var channelId = "firebase_notifications";
                var channel = new NotificationChannel(channelId, "Firebase Notifications", NotificationImportance.High){
                    Description = "Firebase Notifications"
                };
                notificationManager.CreateNotificationChannel(channel);

                var notificationBuilder = new Notification.Builder(this, channelId)
                    .SetContentTitle(title)
                    .SetContentText(body)
                    .SetSmallIcon(Resource.Drawable.ic_notification);

                notificationManager.Notify(0, notificationBuilder.Build());
            }
        }
    