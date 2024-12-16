using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

public interface IAppUserService
{
    Task<ServiceResult<List<AppUserResponseDto>>> GetUsersWithFunctions();
    Task<ServiceResult<bool>> UpdateFcmTokenOnServerAsync(string userId, string token);
    Task<ServiceResult<string>> GetFcmTokenFromServerAsync(string userId);
    Task<ServiceResult<string>> GetUserIdByEmailAsync(string email);
    Task<ServiceResult<string>> GetCurrentUserIdAsync();

}