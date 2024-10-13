using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mde.Project.Mobile.Core.Service.Interfaces;

namespace Mde.Project.Mobile.Core.Service;

public class UiService : IUiService
{
    public async Task ShowSnackbarCreateAsync(string message)
    {
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.DarkSeaGreen,
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            CharacterSpacing = 1
        };

        var snackbar = Snackbar.Make(message, null, "", TimeSpan.FromSeconds(5), snackbarOptions);
        await snackbar.Show();
    }
    
    public async Task ShowSnackbarDeleteAsync(string message)
    {
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.Pink,
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            CharacterSpacing = 1
        };

        var snackbar = Snackbar.Make(message, null, "", TimeSpan.FromSeconds(5), snackbarOptions);
        await snackbar.Show();
    }
    
    
}
