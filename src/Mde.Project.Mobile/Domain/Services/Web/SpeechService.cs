using Mde.Project.Mobile.Domain.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class SpeechService:ISpeechService{
    private readonly SpeechConfig _speechConfig;

    public SpeechService(string subscriptionKey, string region)
    {
        _speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
    }

    public async Task<string> RecognizeSpeechAsync()
    {
        using (var recognizer = new SpeechRecognizer(_speechConfig))
        {
            var result = await recognizer.RecognizeOnceAsync();
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                return result.Text;
            }
            else
            {
                return $"Error: {result.Reason}";
            }
        }
    }
}