using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IAppUserService{
    Task<List<AppUserResponseDto>> GetUsersWithFunctions();

    Task StoreFcmTokenAsync( string token);

    Task<ServiceResult<string>> GetFcmTokenAsync(string userId);

    Task<ServiceResult<string>> GetUserIdByEmailAsync(string email);
}