using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IAppUserService{
    Task<ResultModel<IEnumerable<AppUser>>> GetUsersAsync();
    Task<ResultModel<AppUser>> GetUserByEmailAsync(string email);
    Task<ResultModel<AppUser>> GetUserByIdAsync(string userId);
    Task<ResultModel<AppUser>> UpdateUserAsync(AppUser appUser);
    Task<ResultModel<AppUser>> DeleteUserAsync(string userId);
    Task<ResultModel<string>> GetUserFcmToken(string userId);
  
    Task<ResultModel<string>> UpdateUserPasswordAsync(string userId, string currentPassword, string newPassword);
    Task<ResultModel<string>> UpdateUserFcmToken(string userId, string newToken);
    
  

}