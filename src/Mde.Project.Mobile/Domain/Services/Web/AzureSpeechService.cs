using Mde.Project.Mobile.Domain.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;

namespace Mde.Project.Mobile.Domain.Services;

public class AzureSpeechService : ISpeechService
{
    private readonly string subscriptionKey;
    private readonly string region;

    public AzureSpeechService(string subscriptionKey, string region)
    {
        this.subscriptionKey = subscriptionKey;
        this.region = region;
    }

    public async Task<string> RecognizeSpeechAsync()
    {
        var config = SpeechConfig.FromSubscription(subscriptionKey, region);
        using (var recognizer = new SpeechRecognizer(config))
        {
            var result = await recognizer.RecognizeOnceAsync();
            return result.Reason == ResultReason.RecognizedSpeech ? result.Text : null;
        }
    }
}
