using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Moq;

public class TranslateViewModelTests
{
    private readonly Mock<ISpeechService> _mockSpeechService;
    private readonly Mock<IUiService> _mockUiService;
    private readonly TranslateViewModel _viewModel;

    public TranslateViewModelTests()
    {
        _mockSpeechService = new Mock<ISpeechService>();
        _mockUiService = new Mock<IUiService>();

        _viewModel = new TranslateViewModel(
            _mockSpeechService.Object,
            null,
            null,
            null,
            _mockUiService.Object
        );
    }

    [Fact]
    public async Task StartListeningAsync_SuccessfullyStartsListening()
    {
        // Arrange
        _mockSpeechService.Setup(x => x.SetRecognitionLanguageAsync("nl-NL"))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        _mockSpeechService.Setup(x => x.StartContinuousRecognitionAsync(
                It.IsAny<Action<string>>(),
                It.IsAny<Action<string>>(),
                It.IsAny<Action<string>>()))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        // Act
        await _viewModel.StartListeningAsync();

        // Assert
        Assert.True(_viewModel.IsListening);
        _mockUiService.Verify(x => x.ShowSnackbarWarning(It.IsAny<string>()), Times.Never);
    }
    
  

}