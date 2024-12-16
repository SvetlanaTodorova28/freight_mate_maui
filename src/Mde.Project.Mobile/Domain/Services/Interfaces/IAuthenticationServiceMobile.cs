using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IAuthenticationServiceMobile
{
    /// <summary>
    /// Checks if the user is authenticated.
    /// </summary>
    Task<ServiceResult<bool>> IsAuthenticatedAsync();

    /// <summary>
    /// Tries to log in the user with the provided email and password.
    /// </summary>
    Task<ServiceResult<bool>> TryLoginAsync(string email, string password);

    /// <summary>
    /// Retrieves the stored authentication token.
    /// </summary>
    Task<ServiceResult<string>> GetTokenAsync();

    /// <summary>
    /// Tries to register a new user with the provided information.
    /// </summary>
    Task<ServiceResult<bool>> TryRegisterAsync(AppUser appUser);

    /// <summary>
    /// Retrieves the user ID from the stored authentication token.
    /// </summary>
    Task<ServiceResult<string>> GetUserIdFromTokenAsync();

    /// <summary>
    /// Sends a notification using the specified message payload.
    /// </summary>
    Task<ServiceResult<bool>> SendNotificationAsync(object message);

    /// <summary>
    /// Logs the user out and removes the authentication token.
    /// </summary>
    Task<ServiceResult<bool>> Logout();

    /// <summary>
    /// Retrieves the first name of the user from the authentication token.
    /// </summary>
    Task<ServiceResult<string>> GetUserFirstNameFromTokenAsync();
}