using Microsoft.Maui.Storage;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Helpers
{
    public static class SecureStorageHelper
    {
        public static async Task SaveApiKeyAsync(string key, string value)
        {
            await SecureStorage.SetAsync(key, value);
        }

        public static async Task<string> GetApiKeyAsync(string key)
        {
            return await SecureStorage.GetAsync(key);
        }
    }
}