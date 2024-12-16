using Mde.Project.Mobile.Domain.Services.Web;
#if ANDROID
using Mde.Project.Mobile.Platforms.Android.Listeners;
#endif


namespace Mde.Project.Mobile.Helpers
{
    public static class FirebaseHelper
    {
        public static async Task<ServiceResult<string>> RetrieveAndStoreFirebaseTokenAsync()
        {
#if ANDROID
            try
            {
                var taskCompletionSource = new TaskCompletionSource<string>();

                Firebase.Messaging.FirebaseMessaging.Instance.GetToken()
                    .AddOnSuccessListener(new OnTokenSuccessListener(result =>
                    {
                        taskCompletionSource.SetResult(result);
                    }))
                    .AddOnFailureListener(new OnTokenFailureListener(exception =>
                    {
                        taskCompletionSource.SetException(exception);
                    }));

                var token = await taskCompletionSource.Task;
                return !string.IsNullOrEmpty(token)
                    ? ServiceResult<string>.Success(token)
                    : ServiceResult<string>.Failure("Failed to retrieve FCM token.");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure($"Unexpected error while retrieving FCM token: {ex.Message}");
            }
#else
            return ServiceResult<string>.Failure("FCM Token retrieval is not supported on this platform.");
#endif
        }
    }
}