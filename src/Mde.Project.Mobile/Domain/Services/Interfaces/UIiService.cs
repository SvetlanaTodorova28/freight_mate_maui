
namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IUiService
{
    Task ShowSnackbarSuccessAsync(string message);
    Task ShowSnackbarDeleteAsync(string message);
    Task ShowSnackbarWarning(string message);
}
