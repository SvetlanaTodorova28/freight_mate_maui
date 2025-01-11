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
        return ServiceResult<string>.Failure($"Error retrieving FCM token. Log out and try again .");
    }
#elif __IOS__
    try
    {
        string newToken = Messaging.SharedInstance.FcmToken; 
        string currentToken = await SecureStorageHelper.GetFcmTokenAsync();
        if (!string.IsNullOrEmpty(newToken))
        {
            if(string.IsNullOrEmpty(currentToken) ||!currentToken.Equals(newToken)){
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


       
        public static async Task<ServiceResult<string>> UpdateFcmTokenOnServerAsync(IAppUserService appUserService)
        {
            try
            {
                var localTokenResult = await GetStoredFcmTokenAsync();
                if (!localTokenResult.IsSuccess){
                    var tokenResult = await RetrieveAndStoreFcmTokenLocallyAsync();
                    if (!tokenResult.IsSuccess)
                    {
                        return ServiceResult<string>.Failure("Failed to store FCM token.");
                    }
                }
                
                var userIdResult = await appUserService.GetCurrentUserIdAsync();
                if (!userIdResult.IsSuccess)
                {
                    return ServiceResult<string>.Failure("User ID not found.");
                }

               
                var updateResult = await appUserService.UpdateFcmTokenOnServerAsync(userIdResult.Data, localTokenResult.Data);
                if (!updateResult.IsSuccess)
                {
                    return ServiceResult<string>.Failure(updateResult.ErrorMessage);
                }
                return ServiceResult<string>.Success("Token updated successfully on server.");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure("Error updating FCM token on server.");
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
