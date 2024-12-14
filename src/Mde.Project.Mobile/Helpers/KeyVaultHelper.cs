using System.Net.Http.Json;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos;
using Utilities;

namespace Mde.Project.Mobile.Helpers
{
    public class KeyVaultHelper
    {
        private readonly HttpClient _httpClient;

        public KeyVaultHelper(IHttpClientFactory httpClientFactory){
            _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
        }

       
        public async Task<ServiceResult<bool>> FetchKeysFromApiAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/configuration/azure-keys");

                if (!response.IsSuccessStatusCode)
                {
                    return ServiceResult<bool>.Failure($"Failed to fetch API keys. Status code: {response.StatusCode}");
                }

              
                var keys = await response.Content.ReadFromJsonAsync<ConfigurationResponseDto>();

                if (keys == null)
                {
                    return ServiceResult<bool>.Failure("No keys were returned from the backend.");
                }

               
                if (!string.IsNullOrEmpty(keys.Key_OCR))
                {
                    await SecureStorageHelper.SaveApiKey("Key_OCR", keys.Key_OCR);
                }

                if (!string.IsNullOrEmpty(keys.Key_Speech))
                {
                    await SecureStorageHelper.SaveApiKey("Key_Speech", keys.Key_Speech);
                }

                if (!string.IsNullOrEmpty(keys.Key_Translation))
                {
                    await SecureStorageHelper.SaveApiKey("Key_Translation", keys.Key_Translation);
                }

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Error fetching keys: {ex.Message}");
            }
        }

    }
}
