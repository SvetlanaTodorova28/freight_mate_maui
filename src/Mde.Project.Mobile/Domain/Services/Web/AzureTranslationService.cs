using System.Text;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;

namespace Mde.Project.Mobile.Domain;

public class AzureTranslationService : ITranslationService
{
    private readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
    private readonly string subscriptionKey;
    private readonly string location;

    public AzureTranslationService(string subscriptionKey, string location)
    {
        this.subscriptionKey = subscriptionKey;
        this.location = location;
    }

    public async Task<string> TranslateTextAsync(string text, string targetLanguageCode)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", location);

            var requestBody = JsonConvert.SerializeObject(new[] { new { Text = text } });
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{endpoint}translate?api-version=3.0&to={targetLanguageCode}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<TranslationResult>>(responseBody);

            return result?.FirstOrDefault()?.Translations?.FirstOrDefault()?.Text;
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