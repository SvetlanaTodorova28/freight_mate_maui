using System.Text;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Helpers;
using Newtonsoft.Json;
using Utilities;

namespace Mde.Project.Mobile.Domain;

public class AzureTranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private bool _isInitialized;

    public AzureTranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            var keyTranslation = await SecureStorageHelper.GetApiKeyAsync("Key_Translation");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", keyTranslation);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", GlobalConstants.Region);
            _isInitialized = true;
        }
    }

    public async Task<ServiceResult<string>> TranslateTextAsync(string text, string targetLanguageCode)
    {
        try
        {
            await InitializeAsync();

            var requestBody = new[] { new { Text = text } };
            var jsonContent = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/translate?api-version=3.0&to={targetLanguageCode}", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<TranslationResult>>(json);
                var translatedText = result?.FirstOrDefault()?.Translations?.FirstOrDefault()?.Text;

                if (!string.IsNullOrEmpty(translatedText))
                {
                    return ServiceResult<string>.Success(translatedText);
                }

                return ServiceResult<string>.Failure("Translation result was empty.");
            }

            return ServiceResult<string>.Failure("Translation API request failed with status code. Check the internet connection and try again.");
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure("Unexpected error during translation. Please contact support if the problem persists. ");
        }
    }

    private class TranslationResult
    {
        public List<Translation> Translations { get; set; }
    }

    private class Translation
    {
        public string Text { get; set; }
    }
}
