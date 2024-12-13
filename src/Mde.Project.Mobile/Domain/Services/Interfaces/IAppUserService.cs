using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IAppUserService{
    Task<ServiceResult<List<AppUserResponseDto>>> GetUsersWithFunctions();

    Task<ServiceResult<bool>> StoreFcmTokenAsync(string token);

    Task<ServiceResult<string>> GetFcmTokenAsync(string userId);

    Task<ServiceResult<string>> GetUserIdByEmailAsync(string email);
}