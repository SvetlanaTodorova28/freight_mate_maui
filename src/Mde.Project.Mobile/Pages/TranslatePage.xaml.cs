
using Mde.Project.Mobile.Domain.Services.Interfaces;


namespace Mde.Project.Mobile.Pages;

public partial class TranslatePage : ContentPage{
    private readonly ISpeechService _speechService;
    private readonly ITranslationService _translationService;
    private readonly ITranslationStorageService _translationStorageService;
    private readonly ITextToSpeechService _textToSpeechService;
    private readonly IUiService _uiService;
    
   
    public TranslatePage(ISpeechService speechService, ITranslationService translationService,
        ITranslationStorageService translationStorageService, ITextToSpeechService textToSpeechService, IUiService uiService){
        _speechService = speechService;
        _translationService = translationService;
        _translationStorageService = translationStorageService;
        _textToSpeechService = textToSpeechService; 
        _uiService = uiService;
        InitializeComponent();
    BindingContext = new TranslateViewModel(_speechService, _translationService, _translationStorageService, _textToSpeechService, _uiService);
    }
}