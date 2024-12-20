using Mde.Project.Mobile.Domain.Services.Web;
#if ANDROID
using Mde.Project.Mobile.Platforms.Android.Listeners;
#endif
#if __IOS__
using Firebase.CloudMessaging;
#endif

namespace Mde.Project.Mobile.Helpers
{
    public static class FirebaseHelper
    {
        
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
            await SecureStorageHelper.SaveFcmTokenAsync(token);
            return ServiceResult<string>.Success(token);
        }

        return ServiceResult<string>.Failure("Failed to retrieve FCM token.");
    }
    catch (Exception ex)
    {
        return ServiceResult<string>.Failure($"Error retrieving FCM token: {ex.Message}");
    }
#elif __IOS__
    try
    {
        string token = Messaging.SharedInstance.FcmToken; 
        if (!string.IsNullOrEmpty(token))
        {
            await SecureStorageHelper.SaveFcmTokenAsync(token);
            return ServiceResult<string>.Success(token);
        }
        else
        {
            return ServiceResult<string>.Failure("FCM token is null or empty.");
        }
    }
    catch (Exception ex)
    {
        return ServiceResult<string>.Failure($"Error retrieving FCM token for iOS: {ex.Message}");
    }
#else
    return ServiceResult<string>.Failure("FCM Token retrieval not supported on this platform.");
#endif
}


       
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
