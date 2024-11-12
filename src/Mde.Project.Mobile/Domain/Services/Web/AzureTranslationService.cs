using System.Text;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;

namespace Mde.Project.Mobile.Domain;

public class AzureTranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;

    public AzureTranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> TranslateTextAsync(string text, string targetLanguageCode)
    {
        var requestBody = new[] { new { Text = text } };
        var jsonContent = JsonConvert.SerializeObject(requestBody);
        Console.WriteLine($"Request JSON: {jsonContent}");
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/translate?api-version=3.0&to={targetLanguageCode}", content);
        Console.WriteLine($"Response Status Code: {response.StatusCode}");
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
