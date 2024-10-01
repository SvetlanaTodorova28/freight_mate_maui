namespace Mde.Project.Mobile.Core.Service.Interfaces;

public interface IUiService
{
    Task ShowSnackbarCreateAsync(string message);
    Task ShowSnackbarDeleteAsync(string message);
}
