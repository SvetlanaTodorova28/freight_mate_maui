
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
using Mde.Project.Mobile.ViewModels;
using Moq;

namespace Mde.Project.Mobile.UnitTests
{
    public class CargoCreateViewModelTests
    {
        private readonly Mock<ICargoService> _mockCargoService;
        private readonly Mock<IUiService> _mockUiService;
        private readonly Mock<IAppUserService> _mockAppUserService;
        private readonly Mock<IAuthenticationServiceMobile> _mockAuthService;
        private readonly CargoCreateViewModel _viewModel;

        public CargoCreateViewModelTests()
        {
            _mockCargoService = new Mock<ICargoService>();
            _mockUiService = new Mock<IUiService>();
            _mockAppUserService = new Mock<IAppUserService>();
            _mockAuthService = new Mock<IAuthenticationServiceMobile>();

            _viewModel = new CargoCreateViewModel(
                _mockCargoService.Object,
                _mockUiService.Object,
                _mockAppUserService.Object,
                _mockAuthService.Object
            );
        }

        [Fact]
        public async Task LoadUsersCommand_Executes_LoadsUsersSuccessfully()
        {
            // Arrange
            var users = new List<AppUserResponseDto>()
            {
                new() { Id = Guid.NewGuid(), FirstName = "John" },
                new () { Id = Guid.NewGuid(), FirstName = "Jane" }
            };

            _mockAppUserService
                .Setup(x => x.GetUsersWithFunctions())
                .ReturnsAsync(ServiceResult<List<AppUserResponseDto>>.Success(users));

            // Act
             _viewModel.LoadUsersCommand.Execute(null);

            // Assert
            Assert.Equal(2, _viewModel.Users.Count);
            Assert.Equal("John", _viewModel.Users[0].FirstName);
            Assert.Equal("Jane", _viewModel.Users[1].FirstName);
        }
        
        [Fact]
        public async Task LoadUsers_WhenServiceFails_ShowsWarning()
        {
            // Arrange
            string expectedMessage = "No users found with assigned roles.";
            _mockAppUserService
                .Setup(service => service.GetUsersWithFunctions())
                .ReturnsAsync(ServiceResult<List<AppUserResponseDto>>.Failure(expectedMessage));
        
            // Act
            await _viewModel.LoadUsers();  

            // Assert
            _mockUiService.Verify(x => x.ShowSnackbarWarning(expectedMessage), Times.Once);
        }


        [Fact]
        public async Task SaveCargoCommand_WithValidInput_SavesSuccessfully()
        {
            // Arrange
            _viewModel.Destination = "Amsterdam";
            _viewModel.TotalWeight = 100;
            _viewModel.IsDangerous = true;
            _viewModel.SelectedUser = new AppUserResponseDto { Id = Guid.NewGuid(), FirstName = "John" };

            _mockCargoService
                .Setup(x => x.CreateOrUpdateCargo(It.IsAny<Cargo>(), It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<string>.Success("Cargo saved successfully \ud83d\udce6"));

            // Act
             _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockUiService.Verify(x => x.ShowSnackbarSuccessAsync("Cargo saved successfully 📦"), Times.Once);
        }

        [Fact]
        public async Task SaveCargoCommand_WithInvalidInput_ShowsFailureWarning()
        {
            // Arrange
            _mockCargoService
                .Setup(x => x.CreateOrUpdateCargo(It.IsAny<Cargo>(), It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<string>.Failure("Failed to save cargo"));

            // Act
             _viewModel.SaveCommand.Execute(null);

            // Assert
            _mockUiService.Verify(x => x.ShowSnackbarWarning("Failed to save cargo"), Times.Once);
            Assert.False(_viewModel.IsLoading);
        }
        
    }
}