namespace Mde.Project.Mobile.Helpers
{
    public static class SecureStorageHelper
    {
        private const string TokenKey = "token";
        private const string FcmTokenKey = "fcm_token";

      
        public static async Task SaveApiKey(string key, string value)
        {
            await SecureStorage.SetAsync(key, value);
        }

       
        public static async Task<string> GetApiKeyAsync(string key)
        {
            return await SecureStorage.GetAsync(key);
        }

        
        public static async Task SaveTokenAsync(string token)
        {
            await SecureStorage.SetAsync(TokenKey, token);
        }

       
        public static async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(TokenKey);
        }

       
        public static bool RemoveToken()
        {
            return SecureStorage.Remove(TokenKey);
        }

       
        public static async Task SaveFcmTokenAsync(string fcmToken)
        {
            await SecureStorage.SetAsync(FcmTokenKey, fcmToken);
        }

       
        public static async Task<string> GetFcmTokenAsync()
        {
            return await SecureStorage.GetAsync(FcmTokenKey);
        }

       
        public static bool RemoveFcmToken()
        {
            return SecureStorage.Remove(FcmTokenKey);
        }
    }
}