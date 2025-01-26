using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;

public partial class TranslateViewModel : ObservableObject
{
    private readonly ISpeechService _speechService;
    private readonly ITranslationService _translationService;
    private readonly ITranslationStorageService _translationStorageService;
    private readonly ITextToSpeechService _textToSpeechService;
    private readonly IUiService _uiService;
    private readonly IMainThreadInvoker _mainThreadInvoker;
    private readonly ISnowVisibilityService _snowVisibilityService;
    private readonly IPermissionService _permissionService;

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
        IUiService uiService,
        IMainThreadInvoker mainThreadInvoker,
        ISnowVisibilityService snowVisibilityService,
        IPermissionService permissionService)
    
    {
        _speechService = speechService;
        _translationService = translationService;
        _translationStorageService = translationStorageService;
        _textToSpeechService = textToSpeechService;
        _uiService = uiService;
        _mainThreadInvoker = mainThreadInvoker;
        _snowVisibilityService = snowVisibilityService;
        _permissionService = permissionService;
        UpdateSnowVisibility();
        InitializeSubscriptionSnow();
        
    }
    private bool _snowVisibility;
    public bool SnowVisibility
    {
        get => _snowVisibility;
        private set => SetProperty(ref _snowVisibility, value);
    }

    [RelayCommand]
    public async Task StartListeningAsync(){

        bool hasPermission = await _permissionService
                                 .RequestIfNotGrantedAsync<Permissions.Microphone>() ==
                             PermissionStatus.Granted;
        if (hasPermission){
            string inputLanguageCode = GetLanguageCode(SelectedInputLanguage);
            var setLanguageResult = await _speechService.SetRecognitionLanguageAsync(inputLanguageCode);

            if (!setLanguageResult.IsSuccess){
                _mainThreadInvoker.InvokeOnMainThread(async () => {
                    await _uiService.ShowSnackbarWarning(setLanguageResult.ErrorMessage);
                });
                return;
            }

            IsListening = true;

            await _speechService.StartContinuousRecognitionAsync(
                onRecognized: text => { _mainThreadInvoker.InvokeOnMainThread(() => { RecognizedText = text; }); },
                onError: async error => {
                    _mainThreadInvoker.InvokeOnMainThread(async () => { await _uiService.ShowSnackbarWarning(error); });
                },
                onInfo: async info => {
                    _mainThreadInvoker.InvokeOnMainThread(async () => {
                        await _uiService.ShowSnackbarInfoAsync(info);
                    });
                });
        }
    }



    [RelayCommand]
    public async Task StopListening()
    {
        var stopResult = await _speechService.StopContinuousRecognitionAsync();
        if (stopResult.IsSuccess)
        {
            _mainThreadInvoker.InvokeOnMainThread(() =>
            {
                IsListening = false;
            });
        }
        else
        {
            _mainThreadInvoker.InvokeOnMainThread(async () =>
            {
                await _uiService.ShowSnackbarWarning(stopResult.ErrorMessage);
            });
        }
    }


    [RelayCommand]
    public async Task TranslateSpeechAsync()
    {
        if (string.IsNullOrWhiteSpace(RecognizedText))
        {
            await _uiService.ShowSnackbarWarning("No text to translate.");
            return;
        }

        string targetLanguageCode = GetLanguageCode(SelectedTargetLanguage);
        var translatedTextResult = await _translationService.TranslateTextAsync(RecognizedText, targetLanguageCode);

        if (translatedTextResult.IsSuccess && !string.IsNullOrEmpty(translatedTextResult.Data))
        {
            TranslatedText = translatedTextResult.Data;
            await _translationStorageService.SaveTranslationAsync(new TranslationSpeechModel
            {
                OriginalText = RecognizedText,
                TranslatedText = TranslatedText
            });

            await _uiService.ShowSnackbarSuccessAsync(translatedTextResult.Message);
        }
        else
        {
            await _uiService.ShowSnackbarWarning(translatedTextResult.ErrorMessage);
        }
    }

    [RelayCommand]
    public async Task SpeakTranslatedTextAsync()
    {
        if (string.IsNullOrWhiteSpace(TranslatedText))
        {
            await _uiService.ShowSnackbarWarning("No translated text to speak.");
            return;
        }

        var synthesisResult = await _textToSpeechService.SynthesizeSpeechAsync(TranslatedText);

        if (synthesisResult.IsSuccess)
        {
            await _uiService.ShowSnackbarSuccessAsync(synthesisResult.Message);
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
    
   
    private void InitializeSubscriptionSnow(){
        MessagingCenter.Subscribe<SettingsViewModel, bool>(this, "SnowToggleChanged",  (sender, isEnabled) =>
        {
            UpdateSnowVisibility();
        });
    }
    
    public void UpdateSnowVisibility()
    {
        SnowVisibility = _snowVisibilityService.DetermineSnowVisibility();
    }

   
}
