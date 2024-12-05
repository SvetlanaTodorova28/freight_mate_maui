using Mde.Project.Mobile.Domain.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;

namespace Mde.Project.Mobile.Domain.Services;

public class AzureSpeechService : ISpeechService
{
    private SpeechConfig config;

    public AzureSpeechService(string subscriptionKey, string region)
    {
        config = SpeechConfig.FromSubscription(subscriptionKey, region);
        config.SpeechRecognitionLanguage = "en-US"; 
    }

    public void SetRecognitionLanguage(string languageCode)
    {
        config.SpeechRecognitionLanguage = languageCode;
    }

    public async Task<string> RecognizeSpeechAsync()
    {
        using (var recognizer = new SpeechRecognizer(config))
        {
            var result = await recognizer.RecognizeOnceAsync();
            return result.Reason == ResultReason.RecognizedSpeech ? result.Text : "Error: Speech not recognized";
        }
    }
}