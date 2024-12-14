
namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IUiService
{
    Task ShowSnackbarSuccessAsync(string message);
    Task ShowSnackbarWarning(string message);
    Task<Stream> PickAndOpenFileAsync(string fileFilter);
    Task ShowSnackbarInfoAsync(string message);
}
