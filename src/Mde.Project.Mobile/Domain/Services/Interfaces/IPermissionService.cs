namespace Mde.Project.Mobile.Domain.Services.Interfaces;
public interface IPermissionService
{
    Task<PermissionStatus> CheckAsync<T>()
        where T : Permissions.BasePermission, new();
    Task<PermissionStatus> RequestAsync<T>()
        where T : Permissions.BasePermission, new();
    Task<PermissionStatus> RequestIfNotGrantedAsync<T>()
        where T : Permissions.BasePermission, new();
}