using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IAppUserService{
    Task<List<AppUserResponseDto>> GetUsersWithFunctions();

    Task StoreFcmTokenAsync( string token);

    Task<string> GetFcmTokenAsync(string userId);

    Task<string> GetUserIdByEmailAsync(string email);
}