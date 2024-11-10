using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class TranslationService:ITranslationService{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TranslationService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<string> TranslateTextAsync(string text, string targetLanguageCode)
    {
        var url = $"https://api.example.com/translate?api_key={_apiKey}&text={text}&to={targetLanguageCode}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var translationResult = JsonConvert.DeserializeObject<TranslationResult>(content);
        return translationResult.TranslatedText;
    }
}

public class TranslationResult
{
    public string TranslatedText { get; set; }
}
