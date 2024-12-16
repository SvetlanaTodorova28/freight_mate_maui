using Mde.Project.Mobile.Domain.Services.Interfaces;
namespace Mde.Project.Mobile.Helpers;

public static class StartupHelper
{
    public static async Task InitializeKeyVaultAsync(IServiceProvider services)
    {
        var keyVaultHelper = services.GetRequiredService<KeyVaultHelper>();
        var result = await keyVaultHelper.EnsureKeysAreAvailableAsync();

        if (!result.IsSuccess)
        {
            var uiService = services.GetRequiredService<IUiService>();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                uiService.ShowSnackbarWarning($"Key Vault Error: {result.ErrorMessage}");
            });
        }
    }

  
}
