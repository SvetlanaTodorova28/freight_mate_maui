using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;

public partial class TranslateViewModel : ObservableObject
{
    private readonly ISpeechService _speechService;
    private readonly ITranslationService _translationService;
    private readonly ITranslationStorageService _translationStorageService;
    private readonly ITextToSpeechService _textToSpeechService;
    private readonly IUiService _uiService;

    [ObservableProperty]
    private List<LanguageOption> availableLanguages = new()
    {
        LanguageOption.English,
        LanguageOption.Dutch,
        LanguageOption.French,
        LanguageOption.German,
        LanguageOption.Bulgarian
    };

    [ObservableProperty]
    private LanguageOption selectedInputLanguage = LanguageOption.Dutch;

    [ObservableProperty]
    private LanguageOption selectedTargetLanguage = LanguageOption.French;

    [ObservableProperty]
    private string recognizedText;

    [ObservableProperty]
    private string translatedText;

    [ObservableProperty]
    private bool isListening;

    public TranslateViewModel(
        ISpeechService speechService,
        ITranslationService translationService,
        ITranslationStorageService translationStorageService,
        ITextToSpeechService textToSpeechService,
        IUiService uiService)
    {
        _speechService = speechService;
        _translationService = translationService;
        _translationStorageService = translationStorageService;
        _textToSpeechService = textToSpeechService;
        _uiService = uiService;
    }

    [RelayCommand]
    private async Task StartListeningAsync()
    {
        string inputLanguageCode = GetLanguageCode(SelectedInputLanguage);
        var setLanguageResult = await _speechService.SetRecognitionLanguageAsync(inputLanguageCode);

        if (!setLanguageResult.IsSuccess)
        {
            await _uiService.ShowSnackbarWarning(setLanguageResult.ErrorMessage);
            return;
        }

        IsListening = true;

        await _speechService.StartContinuousRecognitionAsync(
            onRecognized: text =>
            {
                RecognizedText = text;
            },
            onError: async error =>
            {
                await _uiService.ShowSnackbarWarning(error);
            },
            onInfo: async info =>
            {
                await _uiService.ShowSnackbarInfoAsync(info);
            });
    }

    [RelayCommand]
    private async Task StopListeningAsync()
    {
        IsListening = false;

        var stopResult = await _speechService.StopContinuousRecognitionAsync();
        if (!stopResult.IsSuccess)
        {
            await _uiService.ShowSnackbarWarning(stopResult.ErrorMessage);
        }
    }

    [RelayCommand]
    private async Task TranslateSpeechAsync()
    {
        if (string.IsNullOrWhiteSpace(RecognizedText))
        {
            await _uiService.ShowSnackbarWarning("No text to translate.");
            return;
        }

        string targetLanguageCode = GetLanguageCode(SelectedTargetLanguage);
        var translatedTextResult = await _translationService.TranslateTextAsync(RecognizedText, targetLanguageCode);

        if (!translatedTextResult.IsSuccess)
        {
            await _uiService.ShowSnackbarWarning(translatedTextResult.ErrorMessage);
            return;
        }

        TranslatedText = translatedTextResult.Data;

        var saveResult = await _translationStorageService.SaveTranslationAsync(new TranslationSpeechModel
        {
            OriginalText = RecognizedText,
            TranslatedText = TranslatedText
        });

        if (!saveResult.IsSuccess)
        {
            await _uiService.ShowSnackbarWarning(saveResult.ErrorMessage);
            return;
        }

        await _uiService.ShowSnackbarSuccessAsync("Translation completed successfully!");
    }

    [RelayCommand]
    private async Task SpeakTranslatedTextAsync()
    {
        if (string.IsNullOrWhiteSpace(TranslatedText))
        {
            await _uiService.ShowSnackbarWarning("No translated text to speak.");
            return;
        }

        var synthesisResult = await _textToSpeechService.SynthesizeSpeechAsync(TranslatedText);

        if (synthesisResult.IsSuccess)
        {
            await _uiService.ShowSnackbarSuccessAsync("Text-to-speech synthesis completed!");
        }
        else
        {
            await _uiService.ShowSnackbarWarning(synthesisResult.ErrorMessage);
        }
    }

    private string GetLanguageCode(LanguageOption language)
    {
        return language switch
        {
            LanguageOption.English => "en-US",
            LanguageOption.Dutch => "nl-NL",
            LanguageOption.French => "fr-FR",
            LanguageOption.German => "de-DE",
            LanguageOption.Bulgarian => "bg-BG",
            _ => "en-US"
        };
    }
}
