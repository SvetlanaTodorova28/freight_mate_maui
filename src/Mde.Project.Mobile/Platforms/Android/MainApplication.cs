using Android.App;
using Android.Content.Res;
using AndroidColor = Android.Graphics.Color;
using Android.Runtime;
using Firebase;
using Firebase.Messaging;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace Mde.Project.Mobile.Platforms.Android
{
    [Application]
    public class MainApplication : MauiApplication
    {
        
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
                if (view is Entry)
                {
                    handler.PlatformView.Background.SetTint(Colors.Transparent.ToAndroid());
                    handler.PlatformView.TextCursorDrawable.SetTint(Colors.OrangeRed.ToAndroid());
                }
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override void OnCreate()
        {
            base.OnCreate();
            FirebaseApp.InitializeApp(this);
            FirebaseMessaging.Instance.AutoInitEnabled = true;
        }

     
    }
}
