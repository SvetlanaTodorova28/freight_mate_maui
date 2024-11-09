using DotNetEnv;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class AzureSpeechService: ISpeechService{
    public async Task<string> RecognizeSpeechAsync(){
        Env.Load();
        var key = Environment.GetEnvironmentVariable("KEY_SPEECH_1");
        var config = SpeechConfig.FromSubscription("key", "northeurope");
        using (var recognizer = new SpeechRecognizer(config))
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