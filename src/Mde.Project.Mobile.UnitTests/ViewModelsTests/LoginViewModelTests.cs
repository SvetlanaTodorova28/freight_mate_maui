using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using Moq;

namespace Mde.Project.Mobile.UnitTests;
public class LoginViewModelTests
{
    private readonly Mock<IAuthenticationServiceMobile> _mockAuthService = new();
    private readonly Mock<IUiService> _mockUiService = new();
    private readonly Mock<IAppUserService> _mockAppUserService = new();
    private readonly Mock<INativeAuthentication> _mockNativeAuth = new();
    private readonly Mock<IMainThreadInvoker> _mockMainThreadInvoker = new();
    private readonly Mock<IFirebaseTokenService> _mockFirebaseTokenService = new();
    private readonly Mock<IFunctionAccessService> _mockFunctionAccessService = new();
    private readonly LoginViewModel _viewModel;
   

    public LoginViewModelTests()
    {
       
        _viewModel = new LoginViewModel(
            _mockUiService.Object,
            _mockAuthService.Object,
            new AppUserRegisterViewModel( _mockUiService.Object, _mockAuthService.Object, _mockFunctionAccessService.Object),
            _mockNativeAuth.Object,
            _mockAppUserService.Object,
            _mockMainThreadInvoker.Object,
            _mockFirebaseTokenService.Object);
    }
    [Fact]
    public async Task ExecuteLoginCommandAsync_FailedLogin_ShowsErrorMessage()
    {
        // Arrange
        _mockAuthService.Setup(x => x.TryLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ServiceResult<bool>() { IsSuccess = false, ErrorMessage = "Login failed" });

        // Act
        await _viewModel.ExecuteLoginCommandAsync();

        // Assert
        _mockUiService.Verify(ui => ui.ShowSnackbarWarning("Login failed"), Times.Once);
    }
    
    [Fact]
    public async Task ExecuteFaceLoginCommandAsync_SuccessfulBiometricAuthAndLogin()
    {
        // Arrange
        _mockNativeAuth.Setup(x => x.PromptLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(new NativeAuthResult { Authenticated = true });
        _mockAuthService.Setup(x => x.TryLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ServiceResult<bool>() { IsSuccess = true });
        _mockFirebaseTokenService.Setup(x => x.UpdateFcmTokenOnServerAsync(_mockAppUserService.Object))
            .ReturnsAsync(new ServiceResult<string>() { IsSuccess = true });

        // Act
        await _viewModel.ExecuteFaceLoginCommandAsync();

        // Assert based on expected platform support
#if __ANDROID__ || __IOS__
    _mockUiService.Verify(ui => ui.ShowSnackbarWarning(It.IsAny<string>()), Times.Never);
#else
        _mockUiService.Verify(ui => ui.ShowSnackbarWarning("Biometric authentication is not supported on this platform."), Times.Once);
#endif
    }

    
    [Fact]
    public async Task ExecuteFaceLoginCommandAsync_FailedBiometricAuth_ShowsErrorMessage()
    {
        // Arrange
        _mockNativeAuth.Setup(x => x.PromptLoginAsync(It.IsAny<string>()))
            .ReturnsAsync(new NativeAuthResult { Authenticated = false});

        // Act
        await _viewModel.ExecuteFaceLoginCommandAsync();

        // Assert
        _mockUiService.Verify(ui => ui.ShowSnackbarWarning(It.IsAny<string>()), Times.Once);
    }
  





}