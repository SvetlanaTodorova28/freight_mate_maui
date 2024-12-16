namespace Mde.Project.Mobile.Helpers
{
    public static class SecureStorageHelper
    {
        private const string TokenKey = "token";
        private const string FcmTokenKey = "fcm_token";

        // Opslaan van algemene API-key
        public static async Task SaveApiKey(string key, string value)
        {
            await SecureStorage.SetAsync(key, value);
        }

        // Ophalen van algemene API-key
        public static async Task<string> GetApiKeyAsync(string key)
        {
            return await SecureStorage.GetAsync(key);
        }

        // Opslaan van JWT-token
        public static async Task SaveTokenAsync(string token)
        {
            await SecureStorage.SetAsync(TokenKey, token);
        }

        // Ophalen van JWT-token
        public static async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(TokenKey);
        }

        // Verwijderen van JWT-token
        public static bool RemoveToken()
        {
            return SecureStorage.Remove(TokenKey);
        }

        // Opslaan van FCM-token
        public static async Task SaveFcmTokenAsync(string fcmToken)
        {
            await SecureStorage.SetAsync(FcmTokenKey, fcmToken);
        }

        // Ophalen van FCM-token
        public static async Task<string> GetFcmTokenAsync()
        {
            return await SecureStorage.GetAsync(FcmTokenKey);
        }

        // Verwijderen van FCM-token
        public static bool RemoveFcmToken()
        {
            return SecureStorage.Remove(FcmTokenKey);
        }
    }
}