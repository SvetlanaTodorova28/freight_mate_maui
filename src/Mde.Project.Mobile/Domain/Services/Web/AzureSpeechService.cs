using Microsoft.CognitiveServices.Speech;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;
using System;
using System.Threading.Tasks;
using Mde.Project.Mobile.Domain.Services.Web;

public class AzureSpeechService : ISpeechService
{
    private readonly Lazy<Task<SpeechConfig>> _lazyConfig;
    private readonly string _region;
    private SpeechRecognizer _recognizer;

    public AzureSpeechService(string region)
    {
        _region = region;
        _lazyConfig = new Lazy<Task<SpeechConfig>>(InitializeConfigAsync);
    }

    private async Task<SpeechConfig> InitializeConfigAsync()
    {
        var subscriptionKey = await SecureStorageHelper.GetApiKeyAsync("Key_Speech");
        if (string.IsNullOrEmpty(subscriptionKey))
        {
            throw new InvalidOperationException("Speech API key not found in secure storage.");
        }

        return SpeechConfig.FromSubscription(subscriptionKey, _region);
    }

    public async Task<ServiceResult<bool>> SetRecognitionLanguageAsync(string languageCode)
    {
        if (string.IsNullOrEmpty(languageCode))
        {
            return ServiceResult<bool>.Failure("Language code cannot be null or empty.");
        }

        try
        {
            var config = await _lazyConfig.Value;
            config.SpeechRecognitionLanguage = languageCode;
            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Failed to set recognition language: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> StartContinuousRecognitionAsync(Action<string> onRecognized, Action<string> onError, Action<string> onInfo)
    {
        try
        {
            var config = await _lazyConfig.Value;
            _recognizer?.Dispose();
            _recognizer = new SpeechRecognizer(config);

            _recognizer.Recognizing += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Result.Text))
                {
                    onRecognized?.Invoke(e.Result.Text);
                }
            };

            _recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    onRecognized?.Invoke(e.Result.Text);
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    onError?.Invoke("No speech could be recognized.");
                }
            };

            _recognizer.Canceled += (s, e) =>
            {
                onError?.Invoke($"Recognition canceled: {e.Reason} - {e.ErrorDetails}");
            };

            _recognizer.SessionStopped += (s, e) =>
            {
                onInfo?.Invoke("Speech recognition session stopped gracefully.");
            };

            await _recognizer.StartContinuousRecognitionAsync();
            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Failed to start continuous recognition: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> StopContinuousRecognitionAsync()
    {
        if (_recognizer == null)
        {
            return ServiceResult<bool>.Failure("Recognition has not been started yet.");
        }

        try
        {
            await _recognizer.StopContinuousRecognitionAsync();
            _recognizer.Dispose();
            _recognizer = null;

            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Failed to stop recognition: {ex.Message}");
        }
    }
}
