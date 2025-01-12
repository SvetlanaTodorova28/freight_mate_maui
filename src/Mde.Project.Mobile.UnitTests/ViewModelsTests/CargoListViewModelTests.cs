using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using System.Collections.ObjectModel;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Moq;


namespace Mde.Project.Mobile.UnitTests
{
    public class CargoListViewModelTests
    {
        private readonly Mock<ICargoService> _mockCargoService;
        private readonly Mock<IUiService> _mockUiService;
        private readonly Mock<IAuthenticationServiceMobile> _mockAuthService;
        private readonly Mock<IFunctionAccessService> _mockFunctionAccessService;
        private readonly Mock<ISnowVisibilityService> _mockSnowVisibilityService;
        private readonly CargoListViewModel _viewModel;

        public CargoListViewModelTests()
        {
            _mockCargoService = new Mock<ICargoService>();
            _mockUiService = new Mock<IUiService>();
            _mockAuthService = new Mock<IAuthenticationServiceMobile>();
            _mockFunctionAccessService = new Mock<IFunctionAccessService>();
            _mockSnowVisibilityService = new Mock<ISnowVisibilityService>();

            _viewModel = new CargoListViewModel(
                _mockCargoService.Object,
                _mockUiService.Object,
                _mockAuthService.Object,
                _mockFunctionAccessService.Object,
                _mockSnowVisibilityService.Object
            );
        }

        [Fact]
        public async Task RefreshListCommand_WhenCalled_LoadsCargosSuccessfully()
        {
            // Arrange
            var cargos = new List<Cargo>
            {
                new () { Id = Guid.NewGuid(), Destination = "Amsterdam", IsDangerous = false, TotalWeight = 100 },
                new () { Id = Guid.NewGuid(), Destination = "Rotterdam", IsDangerous = true, TotalWeight = 200 }
            };

            var userId = Guid.NewGuid().ToString();

            _mockAuthService.Setup(x => x.GetUserIdFromTokenAsync())
                .ReturnsAsync(ServiceResult<string>.Success(userId));

            _mockCargoService.Setup(x => x.GetCargosForUser(It.IsAny<Guid>()))
                .ReturnsAsync(ServiceResult<List<CargoResponseDto>>.Success(new List<CargoResponseDto>
                {
                    new () { Id = cargos[0].Id, Destination = cargos[0].Destination, IsDangerous = cargos[0].IsDangerous, TotalWeight = cargos[0].TotalWeight },
                    new () { Id = cargos[1].Id, Destination = cargos[1].Destination, IsDangerous = cargos[1].IsDangerous, TotalWeight = cargos[1].TotalWeight }
                }));
            
            _mockSnowVisibilityService.Setup(x => x.DetermineSnowVisibility()).Returns(true);

            // Act
            _viewModel.RefreshListCommand.Execute(null);

         
            Assert.Equal(2, _viewModel.Cargos.Count);
            Assert.Equal("Amsterdam", _viewModel.Cargos[0].Destination);
            Assert.Equal("Rotterdam", _viewModel.Cargos[1].Destination);
            Assert.False(_viewModel.IsLoading);
        }


        [Fact]
        public async Task RefreshListCommand_WhenAuthServiceFails_ShowsSnackbarWithError()
        {
            // Arrange
            _mockAuthService.Setup(x => x.GetUserIdFromTokenAsync())
                .ReturnsAsync(ServiceResult<string>.Failure("Failed to get user ID."));
            _mockSnowVisibilityService.Setup(x => x.DetermineSnowVisibility()).Returns(true);

            // Act
             _viewModel.RefreshListCommand.Execute(null);

            // Assert
            _mockUiService.Verify(x => x.ShowSnackbarWarning("Failed to get user ID."), Times.Once);
            Assert.Empty(_viewModel.Cargos);
        }
        

        [Fact]
        public async Task DeleteCargoCommand_WhenCargoExists_RemovesItSuccessfully()
        {
            // Arrange
            var cargo = new Cargo { Id = Guid.NewGuid(), Destination = "Test Destination" , IsDangerous = true, TotalWeight = 100 };
            _viewModel.Cargos.Add(cargo);

            _mockCargoService.Setup(x => x.DeleteCargo(It.IsAny<Guid>()))
                .ReturnsAsync(ServiceResult<string>.Success("Cargo deleted successfully"));

            _mockSnowVisibilityService.Setup(x => x.DetermineSnowVisibility()).Returns(true);
            
            // Act
             _viewModel.DeleteCargoCommand.Execute(cargo);

            // Assert
            Assert.DoesNotContain(cargo, _viewModel.Cargos);
           
        }

        [Fact]
        public async Task PerformSearchCommand_WithValidQuery_FiltersCargosCorrectly()
        {
            // Arrange
            _viewModel.Cargos = new ObservableCollection<Cargo>
            {
                new () { Destination = "Amsterdam" },
                new () { Destination = "Rotterdam" },
                new () { Destination = "Utrecht" }
            };
            _mockSnowVisibilityService.Setup(x => x.DetermineSnowVisibility()).Returns(true);

            // Act
             _viewModel.PerformSearchCommand.Execute("Amsterdam");

            // Assert
            Assert.Single(_viewModel.Cargos);
            Assert.Equal("Amsterdam", _viewModel.Cargos[0].Destination);
        }
    }
}