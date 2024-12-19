using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IAuthenticationServiceMobile
{
  
    Task<ServiceResult<bool>> IsAuthenticatedAsync();

  
    Task<ServiceResult<bool>> TryLoginAsync(string email, string password);

  
    Task<ServiceResult<string>> GetTokenAsync();

   
    Task<ServiceResult<bool>> TryRegisterAsync(AppUser appUser);

   
    Task<ServiceResult<string>> GetUserIdFromTokenAsync();

   
    Task<ServiceResult<bool>> SendNotificationAsync(object message);

  
    Task<ServiceResult<bool>> Logout();

   
    Task<ServiceResult<string>> GetUserFirstNameFromTokenAsync();
}