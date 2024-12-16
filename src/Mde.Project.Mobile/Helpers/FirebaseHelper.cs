using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Platforms.Listeners;
    
namespace Mde.Project.Mobile.Helpers;
public static class FirebaseHelper
{
    public static async Task<ServiceResult<bool>> RetrieveAndStoreFirebaseTokenAsync(IAppUserService appUserService)
    {
        try
        {
            var tokenResult = await RetrieveFirebaseTokenAsync();
            if (!tokenResult.IsSuccess)
            {
                return ServiceResult<bool>.Failure(tokenResult.ErrorMessage);
            }

            return await StoreFcmTokenAsync(appUserService, tokenResult.Data);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Unexpected error while retrieving and storing FCM token: {ex.Message}");
        }
    }

    public static async Task<ServiceResult<string>> RetrieveFirebaseTokenAsync()
    {
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
    }

    public static async Task<ServiceResult<bool>> StoreFcmTokenAsync(IAppUserService appUserService, string token)
    {
        try
        {
            var existingToken = await SecureStorageHelper.GetApiKeyAsync("FCM_Token");

            if (!string.IsNullOrEmpty(existingToken) && existingToken == token)
            {
                return ServiceResult<bool>.Success(true); 
            }

            var userIdResult = await appUserService.GetCurrentUserIdAsync();
            if (!userIdResult.IsSuccess)
            {
                return ServiceResult<bool>.Failure(userIdResult.ErrorMessage);
            }

            var response = await appUserService.UpdateFcmTokenOnServerAsync(userIdResult.Data, token);
            if (response.IsSuccess)
            {
                await SecureStorageHelper.SaveApiKey("FCM_Token", token);
                return ServiceResult<bool>.Success(true);
            }

            return ServiceResult<bool>.Failure(response.ErrorMessage);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Unexpected error while storing FCM token: {ex.Message}");
        }
    }
}
