using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mde.Project.Mobile.Domain.Services.Interfaces;


namespace Mde.Project.Mobile.Domain.Services.Web;

public class UiService : IUiService
{
    public async Task ShowSnackbarSuccessAsync(string message)
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
    public async Task ShowSnackbarWarning(string message)
    {
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.LightCoral,
            TextColor = Colors.White,
            Font =Microsoft.Maui.Font.SystemFontOfSize(18),
            CornerRadius = new CornerRadius(10),
            CharacterSpacing = 1
        };

        var snackbar = Snackbar.Make(message, null, "", TimeSpan.FromSeconds(5), snackbarOptions);
        await snackbar.Show();
    }
    public async Task<Stream> PickAndOpenFileAsync(string fileFilter){
        try
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a PDF file",
                FileTypes = FilePickerFileType.Pdf
            });

            if (fileResult != null)
            {
                return await fileResult.OpenReadAsync();
            }
        }
        catch (Exception ex)
        {
            // Log or handle exceptions as necessary
            Console.WriteLine("Error accessing file: " + ex.Message);
        }
        return null;
    }
    public async Task ShowSnackbarInfoAsync(string message)
    {
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.CornflowerBlue, 
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            CharacterSpacing = 1
        };

        var snackbar = Snackbar.Make(message, null, "", TimeSpan.FromSeconds(5), snackbarOptions);
        await snackbar.Show();
    }

    }



