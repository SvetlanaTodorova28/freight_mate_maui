using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.Core.Service.Interfaces
{
    public interface IAuthenticationServiceMobile
    {
        Task<bool> IsAuthenticatedAsync();
        Task<bool> TryLoginAsync(string email, string password);
       Task<bool> TryRegisterAsync(string username, string password, string firstname, string lastname, Function function);
        Task<string> GetTokenAsync();
        bool Logout();

        Task<IEnumerable<Function>> GetFunctionsAsync();
    }
}
