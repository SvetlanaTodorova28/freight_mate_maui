using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class PermissionService : IPermissionService
{
    public Task<PermissionStatus> CheckAsync<T>()
        where T : Permissions.BasePermission, new()
    {
        return Permissions.CheckStatusAsync<T>();
    }
    public Task<PermissionStatus> RequestAsync<T>()
        where T : Permissions.BasePermission, new()
    {
        return Permissions.RequestAsync<T>();
    }
    public async Task<PermissionStatus> RequestIfNotGrantedAsync<T>()
        where T : Permissions.BasePermission, new()
    {
        var status = await CheckAsync<T>();
        if (status != PermissionStatus.Granted)
        {
            return await RequestAsync<T>();
        }
        return status;
    }
}