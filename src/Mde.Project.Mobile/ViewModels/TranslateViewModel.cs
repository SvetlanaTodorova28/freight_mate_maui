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

    [ObservableProperty]
    private List<LanguageOption> availableLanguages = new List<LanguageOption>
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
    private string translatedText;

    [ObservableProperty]
    private bool isListening;

    public TranslateViewModel(ISpeechService speechService, ITranslationService translationService, 
                              ITranslationStorageService translationStorageService, ITextToSpeechService textToSpeechService)
    {
        _speechService = speechService;
        _translationService = translationService;
        _translationStorageService = translationStorageService;
        _textToSpeechService = textToSpeechService;

        StartListeningCommand = new AsyncRelayCommand(StartListeningAsync);
        StopListeningCommand = new RelayCommand(StopListening);
        TranslateCommand = new AsyncRelayCommand(TranslateSpeechAsync);
        SpeakTranslatedTextCommand = new AsyncRelayCommand(SpeakTranslatedTextAsync);
    }

    public IAsyncRelayCommand StartListeningCommand { get; }
    public RelayCommand StopListeningCommand { get; }
    public IAsyncRelayCommand TranslateCommand { get; }
    public IAsyncRelayCommand SpeakTranslatedTextCommand { get; }

    private async Task StartListeningAsync()
    {
        
        string inputLanguageCode = GetLanguageCode(selectedInputLanguage);
        _speechService.SetRecognitionLanguage(inputLanguageCode);

        IsListening = true;
        OnPropertyChanged(nameof(IsListening));

        var speechText = await _speechService.RecognizeSpeechAsync();
        if (!string.IsNullOrEmpty(speechText))
        {
            TranslatedText = speechText;
        }
        else
        {
            TranslatedText = "Could not recognize speech.";
        }

        IsListening = false;
        OnPropertyChanged(nameof(IsListening));
    }

    private void StopListening()
    {
        IsListening = false;
    }

    private async Task TranslateSpeechAsync()
    {
        if (!string.IsNullOrEmpty(TranslatedText))
        {
            
            string targetLanguageCode = GetLanguageCode(selectedTargetLanguage);
            var translatedText = await _translationService.TranslateTextAsync(TranslatedText, targetLanguageCode);
            TranslatedText = translatedText;
            await _translationStorageService.SaveTranslationAsync(new TranslationSpeechModel { OriginalText = TranslatedText, TranslatedText = translatedText });
        }
        else
        {
            TranslatedText = "No text to translate.";
        }
    }
    private async Task SpeakTranslatedTextAsync()
    {
        if (!string.IsNullOrEmpty(TranslatedText))
        {
            await _textToSpeechService.SynthesizeSpeechAsync(TranslatedText);
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
