using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

    public interface IAuthenticationServiceMobile
    {
        Task<bool> IsAuthenticatedAsync();
        Task<bool> TryLoginAsync(string email, string password);

        public Task<string> GetTokenAsync();
     
       Task<bool> TryRegisterAsync(AppUser appUser);

       Task<string> GetUserIdFromTokenAsync();
    
       Task SendNotificationAsync(object message);
        bool Logout();
        
       Task<string> GetUserFirstNameFromTokenAsync();
       
    }

