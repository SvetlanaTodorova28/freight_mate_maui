using Microsoft.CognitiveServices.Speech;

using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services
{
    public class AzureTextToSpeechService : ITextToSpeechService
    {
        private readonly SpeechConfig _config;

        public AzureTextToSpeechService(string subscriptionKey, string region)
        {
            _config = SpeechConfig.FromSubscription(subscriptionKey, region);
        }

        public async Task SynthesizeSpeechAsync(string text)
        {
            using (var synthesizer = new SpeechSynthesizer(_config))
            {
                var result = await synthesizer.SpeakTextAsync(text);
                if (result.Reason != ResultReason.SynthesizingAudioCompleted)
                {
                    throw new ApplicationException($"Speech synthesis failed: {result.Reason}");
                }
            }
        }
    }
}