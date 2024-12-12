using Microsoft.CognitiveServices.Speech;

using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;

namespace Mde.Project.Mobile.Domain.Services
{
    public class AzureTextToSpeechService : ITextToSpeechService
    {
        private Lazy<Task<SpeechConfig>> _lazyConfig;
        private readonly string region;

        public AzureTextToSpeechService(string region)
        {
            this.region = region;
            _lazyConfig = new Lazy<Task<SpeechConfig>>(InitializeConfigAsync);
        }
        private async Task<SpeechConfig> InitializeConfigAsync()
        {
            string subscriptionKey = await SecureStorageHelper.GetApiKeyAsync("Key_Speech");
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            return config;
        }

        

        public async Task SynthesizeSpeechAsync(string text)
        {
            var config = await _lazyConfig.Value;
            using (var synthesizer = new SpeechSynthesizer(config))
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