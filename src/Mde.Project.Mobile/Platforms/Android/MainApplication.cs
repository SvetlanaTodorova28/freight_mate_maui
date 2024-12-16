using Android.App;
using Android.Content.Res;
using AndroidColor = Android.Graphics.Color;
using Android.Runtime;
using Firebase;
using Firebase.Messaging;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Platforms.Android
{
    [Application]
    public class MainApplication : MauiApplication
    {
        private readonly IServiceProvider _serviceProvider;

        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
                if (view is Entry)
                {
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(AndroidColor.Transparent);
                }
            });

            // Initialiseer de serviceprovider hier één keer
            _serviceProvider = MauiProgram.CreateMauiApp().Services;
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override void OnCreate()
        {
            base.OnCreate();

            // Initialize Firebase
            FirebaseApp.InitializeApp(this);
            FirebaseMessaging.Instance.AutoInitEnabled = true;

            // Haal de FCM-token op en sla deze op
            InitializeFirebaseTokenAsync().ConfigureAwait(false);
        }

        private async Task InitializeFirebaseTokenAsync()
        {
            // Haal services op via de serviceprovider
            var appUserService = _serviceProvider.GetService<IAppUserService>();
            var uiService = _serviceProvider.GetService<IUiService>();

            if (appUserService == null || uiService == null)
            {
                await uiService?.ShowSnackbarWarning("Error initializing Firebase token: Missing services.");
                return;
            }

            var result = await FirebaseHelper.RetrieveAndStoreFirebaseTokenAsync();

            if (!result.IsSuccess)
            {
                await uiService.ShowSnackbarWarning(result.ErrorMessage);
            }
        }
    }
}
