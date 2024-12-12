using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;
using Microsoft.CognitiveServices.Speech;

namespace Mde.Project.Mobile.Domain.Services;

public class AzureSpeechService : ISpeechService
{
    private Lazy<Task> initializeConfigTask;
    private string subscriptionKey;
    private SpeechConfig config;
    private readonly string region;


    public AzureSpeechService(string region)
    {
        this.region = region;
        initializeConfigTask = new Lazy<Task>(InitializeConfigAsync);
    }
    private async Task InitializeConfigAsync()
    {
        subscriptionKey = await SecureStorageHelper.GetApiKeyAsync("Key_Speech");
        config = SpeechConfig.FromSubscription(subscriptionKey, region);
    }


    public async Task SetRecognitionLanguage(string languageCode)
    {
        await initializeConfigTask.Value;
        config.SpeechRecognitionLanguage = languageCode;
    }

    public async Task<string> RecognizeSpeechAsync()
    {
        await initializeConfigTask.Value;
        using (var recognizer = new SpeechRecognizer(config))
        {
            var result = await recognizer.RecognizeOnceAsync();
            return result.Reason == ResultReason.RecognizedSpeech ? result.Text : "Error: Speech not recognized";
        }
    }
}