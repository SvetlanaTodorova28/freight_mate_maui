using Mde.Project.Mobile.Domain.Services.Interfaces;
using Moq;

namespace Mde.Project.Mobile.UnitTests;

public class SettingsViewModelTests
{
    [Fact]
    public void TestSnowEnabledProperty()
    {
        var mockPreferencesService = new Mock<IPreferencesService>();
        mockPreferencesService.Setup(x => x.GetBoolean("SnowEnabled", false)).Returns(true);  

        var viewModel = new SettingsViewModel(mockPreferencesService.Object);  

        Assert.True(viewModel.SnowEnabled);  

        viewModel.SnowEnabled = false;
        mockPreferencesService.Verify(x => x.SetBoolean("SnowEnabled", false), Times.Once);  
    }
}
