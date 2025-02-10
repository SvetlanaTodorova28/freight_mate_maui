using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using Moq;

namespace Mde.Project.Mobile.UnitTests
{
    public class CargoDetailsViewModelTests
    {
        private readonly Mock<IGeocodingService> _mockGeocodingService;
        private readonly Mock<IPermissionService> _mockPermissionService;
        private readonly CargoDetailsViewModel _viewModel;

        public CargoDetailsViewModelTests()
        {
            _mockGeocodingService = new Mock<IGeocodingService>();
            _viewModel = new CargoDetailsViewModel(_mockGeocodingService.Object, _mockPermissionService.Object);
        }

        [Fact]
        public void SelectedCargo_SetsPropertiesCorrectly()
        {
            // Arrange
            var destination = "123 Test Street";

            // Act
            _viewModel.Destination = destination;

            // Assert
            Assert.Equal("123 Test Street", _viewModel.Destination);
        }

      //  [Fact]
        /*public async Task NavigateCommand_OpensMapWhenLocationFound()
        {
            // Arrange
            _mockGeocodingService
                .Setup(x => x.GetLocationsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Location> { new (51.509865, -0.118092) });
            _mockPermissionService.Setup(x => x.RequestPermissionAsync<Permissions.LocationWhenInUse>(
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(true);

            // Act
            _viewModel.NavigateCommand.Execute("London");

            // Assert
            _mockGeocodingService.Verify(x => x.GetLocationsAsync("London"), Times.Once);
        }*/

        [Fact]
        public async Task NavigateCommand_DoesNotOpenMap_WhenLocationNotFound()
        {
            // Arrange
            _mockGeocodingService
                .Setup(x => x.GetLocationsAsync(It.IsAny<string>()))
                .ReturnsAsync(Enumerable.Empty<Location>());

            // Act
            _viewModel.NavigateCommand.Execute("Invalid Address");

            // Assert
            _mockGeocodingService.Verify(x => x.GetLocationsAsync("Invalid Address"), Times.Once);
        }
    }
}