using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

    public interface IAuthenticationServiceMobile
    {
        Task<bool> IsAuthenticatedAsync();
        Task<bool> TryLoginAsync(string email, string password);
     
       Task<bool> TryRegisterAsync(string username, string password, string confirmPassword, string firstname, string lastname, Function function);

       Task<string> GetUserIdFromTokenAsync();
        bool Logout();

        Task<IEnumerable<Function>> GetFunctionsAsync();

       Task<string> GetUserFirstNameFromTokenAsync();
       
        Task<Function> GetUserFunctionFromTokenAsync();
    }

