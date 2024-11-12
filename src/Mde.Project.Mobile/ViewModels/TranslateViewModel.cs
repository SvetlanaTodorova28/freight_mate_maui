using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class TranslateViewModel : ObservableObject
{
    private readonly ISpeechService _speechService;
    private readonly ITranslationService _translationService;
    private readonly ITranslationStorageService _translationStorageService;

    [ObservableProperty]
    private List<string> availableLanguages = new List<string> { "en-US", "nl-NL", "fr-FR", "de-DE","bg-BG" }; 

    [ObservableProperty]
    private string selectedInputLanguage;

    [ObservableProperty]
    private string selectedTargetLanguage;

    [ObservableProperty]
    private string translatedText;

    [ObservableProperty]
    private bool isListening;

    public TranslateViewModel(ISpeechService speechService, ITranslationService translationService, 
                              ITranslationStorageService translationStorageService)
    {
        _speechService = speechService;
        _translationService = translationService;
        _translationStorageService = translationStorageService;

        SelectedInputLanguage = "nl-NL"; 
        SelectedTargetLanguage = "fr-FR"; 
        StartListeningCommand = new AsyncRelayCommand(StartListeningAsync);
        StopListeningCommand = new RelayCommand(StopListening);
        TranslateCommand = new AsyncRelayCommand(TranslateSpeechAsync);
    }

    public IAsyncRelayCommand StartListeningCommand { get; }
    public RelayCommand StopListeningCommand { get; }
    public IAsyncRelayCommand TranslateCommand { get; }

    private async Task StartListeningAsync()
    {
        
        _speechService.SetRecognitionLanguage(SelectedInputLanguage);

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
            var translatedText = await _translationService.TranslateTextAsync(TranslatedText, SelectedTargetLanguage);
            TranslatedText = translatedText;
            await _translationStorageService.SaveTranslationAsync(new TranslationSpeechModel { OriginalText = TranslatedText, TranslatedText = translatedText });
        }
        else
        {
            TranslatedText = "No text to translate.";
        }
    }
}
