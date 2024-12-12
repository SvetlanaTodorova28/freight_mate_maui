using System.Text;
using DotNetEnv;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;
using Newtonsoft.Json;
using Utilities;

namespace Mde.Project.Mobile.Domain;

public class AzureTranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private bool _isInitialized ;

    public AzureTranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
       
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized){
            var keyTranslation = await SecureStorageHelper.GetApiKeyAsync("Key_Translation");
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", keyTranslation);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", GlobalConstants.Region);
            _isInitialized = true;
        }
    }
    public async Task<string> TranslateTextAsync(string text, string targetLanguageCode)
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
            return result?.FirstOrDefault()?.Translations?.FirstOrDefault()?.Text;
        }
        return null;
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
