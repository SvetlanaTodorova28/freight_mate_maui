
using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.ViewModels;

public class SpeechViewModel{
    private readonly ISpeechService _speechService;
    private readonly ITranslationStorageService _translationStorageService;
    private readonly ITranslationService _translationService;

    public SpeechViewModel(ISpeechService speechService, ITranslationStorageService translationStorageService, ITranslationService translationService)
    {
        _speechService = speechService;
        _translationStorageService = translationStorageService;
        _translationService = translationService;
    }

    public async Task<string> GetSpeechAndTranslate(string targetLanguageCode)
    {
        string speechText = await _speechService.RecognizeSpeechAsync();
        if (!string.IsNullOrEmpty(speechText))
        {
            return await _translationService.TranslateTextAsync(speechText, targetLanguageCode);
        }
        return "Speech not recognized or error.";
    }
   

}