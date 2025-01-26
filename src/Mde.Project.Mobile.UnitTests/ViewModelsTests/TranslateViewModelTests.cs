using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Moq;


public class TranslateViewModelTests
{
    private readonly Mock<ISpeechService> _mockSpeechService;
    private readonly Mock<IUiService> _mockUiService;
    private readonly Mock<ITranslationService> _mockTranslationService;
    private readonly Mock<IMainThreadInvoker> _mockMainThreadInvoker; 
    private readonly Mock<ITranslationStorageService> _mockTranslationStorageService; 
    private readonly Mock<ISnowVisibilityService> _mockSnowVisibilityService;
    private readonly Mock<IPermissionService> _mockPermissionService;
    private readonly TranslateViewModel _viewModel;

    public TranslateViewModelTests()
    {
        _mockSpeechService = new Mock<ISpeechService>();
        _mockUiService = new Mock<IUiService>();
        _mockMainThreadInvoker = new Mock<IMainThreadInvoker>(); 
        _mockTranslationService = new Mock<ITranslationService>();
        _mockTranslationStorageService = new Mock<ITranslationStorageService>();
        _mockSnowVisibilityService = new Mock<ISnowVisibilityService>(); 
        _mockPermissionService = new Mock<IPermissionService>();

        _viewModel = new TranslateViewModel(
            _mockSpeechService.Object,
            null,
            null, 
            null,
            _mockUiService.Object,
            _mockMainThreadInvoker.Object,
            _mockSnowVisibilityService.Object,
            _mockPermissionService.Object
        );
    }

    [Fact]
    public async Task StartListeningAsync_SuccessfullyStartsListening()
    {
        // Arrange
        _mockSpeechService
            .Setup(x => x.SetRecognitionLanguageAsync("nl-NL"))
            .ReturnsAsync(ServiceResult<bool>.Success(true));
        _mockSpeechService.Setup(x => x.StartContinuousRecognitionAsync(
                It.IsAny<Action<string>>(),
                It.IsAny<Action<string>>(),
                It.IsAny<Action<string>>()))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

       
        _mockMainThreadInvoker.Setup(invoker => invoker.InvokeOnMainThread(It.IsAny<Action>()))
            .Callback<Action>(action => action());
        
        _mockSnowVisibilityService.Setup(x => x.DetermineSnowVisibility()).Returns(true);
        
        _mockPermissionService.Setup(x => x.RequestPermissionAsync<Permissions.Microphone>(
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(true);


        // Act
        await _viewModel.StartListeningAsync();

        // Assert
        Assert.True(_viewModel.IsListening);
       _mockUiService
           .Verify(x => x.ShowSnackbarWarning(It.IsAny<string>()), Times.Never);
    }
  


    

    [Fact]
    public async Task TranslateSpeech_WithNoText_ShowsWarning()
    {
        // Arrange
        _viewModel.RecognizedText = "";
        _mockSnowVisibilityService.Setup(x => x.DetermineSnowVisibility()).Returns(true);


        // Act
        await _viewModel.TranslateSpeechAsync();

        // Assert
        _mockUiService.Verify(x => x.ShowSnackbarWarning("No text to translate."), Times.Once);
    }

  

    
}
