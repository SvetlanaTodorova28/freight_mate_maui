
using Mde.Project.Mobile.Domain.Models;
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
            string translatedText = await _translationService.TranslateTextAsync(speechText, targetLanguageCode);
            // Opslaan van de vertaling
            var translationModel = new TranslationSpeechModel
            {
                OriginalText = speechText,
                TranslatedText = translatedText,
                TargetLanguage = targetLanguageCode
            };
            await _translationStorageService.SaveTranslationAsync(translationModel);

            // Optioneel: Converteer de vertaalde tekst naar spraak
            // Dit zou een andere service kunnen zijn of een uitbreiding van je huidige diensten
            /*await SpeakText(translatedText, targetLanguageCode);*/
            return translatedText;
        }
        return "Speech not recognized or error.";
    }

}