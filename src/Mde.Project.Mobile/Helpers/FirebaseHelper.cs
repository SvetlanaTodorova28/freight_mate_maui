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

        var newToken = await taskCompletionSource.Task;
       
        if (!string.IsNullOrEmpty(newToken))
        {
            var currentToken = await SecureStorageHelper.GetFcmTokenAsync();
            if (!newToken.Equals(currentToken)){
                await SecureStorageHelper.SaveFcmTokenAsync(newToken);
                return ServiceResult<string>.Success(newToken);
            }
            return ServiceResult<string>.Success("Token unchanged.");
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
        string newToken = Messaging.SharedInstance.FcmToken; 
        string currentToken = await SecureStorageHelper.GetFcmTokenAsync();
        if (!string.IsNullOrEmpty(newToken))
        {
            if (!newToken.Equals(currentToken))
            {
                await SecureStorageHelper.SaveFcmTokenAsync(newToken);
                return ServiceResult<string>.Success(newToken);
            }
            return ServiceResult<string>.Success("Token unchanged.");
        }
        else
        {
            return ServiceResult<string>.Failure("FCM token is null or empty.");
        }
    }
    catch (Exception ex)
    {
        return ServiceResult<string>.Failure($"Error retrieving FCM token for iOS. Please contact support if the problem persists.");
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
                return ServiceResult<bool>.Failure($"Error updating FCM token on server.");
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
                return ServiceResult<string>.Failure($"Error retrieving stored FCM token.");
            }
        }
    }
}
