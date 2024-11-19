
namespace Mde.Project.Mobile.Helpers
{
    public static class SecureStorageHelper
    {
        public static void SaveApiKey(string key, string value)
        {
            Task.Run(async () => await SecureStorage.SetAsync(key, value)).Wait();
        }

        public static string GetApiKey(string key)
        {
            return Task.Run(async () => await SecureStorage.GetAsync(key)).Result;
        }
    }

}