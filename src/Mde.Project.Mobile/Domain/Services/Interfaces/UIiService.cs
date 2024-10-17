namespace Mde.Project.Mobile.Core.Service.Interfaces;

public interface IUiService
{
    Task ShowSnackbarSuccessAsync(string message);
    Task ShowSnackbarDeleteAsync(string message);
    Task ShowSnackbarWarning(string message);
}
