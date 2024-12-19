using Mde.Project.Mobile.Domain.Services.Web;


#if ANDROID
using Mde.Project.Mobile.Platforms.Android.Listeners;
#endif

namespace Mde.Project.Mobile.Helpers
{
    public static class FirebaseHelper
    {
        // Stap 1: Haal en sla de FCM-token lokaal op
        public static async Task<ServiceResult<string>> RetrieveAndStoreFcmTokenLocallyAsync()
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

                if (!string.IsNullOrEmpty(token))
                {
                    //fetch en sla de token lokaal op
                    await SecureStorageHelper.SaveFcmTokenAsync(token);
                    return ServiceResult<string>.Success(token);
                }

                return ServiceResult<string>.Failure("Failed to retrieve FCM token.");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure($"Error retrieving FCM token: {ex.Message}");
            }
#else
            return ServiceResult<string>.Failure("FCM Token retrieval not supported on this platform.");
#endif
        }

        // Stap 2: Stuur de FCM-token naar de server na login
        public static async Task<ServiceResult<bool>> UpdateFcmTokenOnServerAsync(IAppUserService appUserService)
        {
            try
            {
               
                var localTokenResult = await GetStoredFcmTokenAsync();
                if (!localTokenResult.IsSuccess)
                {
                    return ServiceResult<bool>.Failure(localTokenResult.ErrorMessage);
                }
                
                var userIdResult = await appUserService.GetCurrentUserIdAsync();
                if (!userIdResult.IsSuccess)
                {
                    return ServiceResult<bool>.Failure("User ID not found.");
                }

               
                var updateResult = await appUserService.UpdateFcmTokenOnServerAsync(userIdResult.Data, localTokenResult.Data);
                return updateResult;
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Error updating FCM token on server: {ex.Message}");
            }
        }

        public static async Task<ServiceResult<string>> GetStoredFcmTokenAsync()
        {
            try
            {
                var token = await SecureStorageHelper.GetFcmTokenAsync();
                return !string.IsNullOrEmpty(token)
                    ? ServiceResult<string>.Success(token)
                    : ServiceResult<string>.Failure("No FCM token found in local storage.");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure($"Error retrieving stored FCM token: {ex.Message}");
            }
        }
    }
}
