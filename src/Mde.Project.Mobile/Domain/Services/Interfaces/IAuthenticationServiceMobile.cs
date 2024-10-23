namespace Mde.Project.Mobile.Core.Service.Interfaces
{
    public interface IAuthenticationServiceMobile
    {
        Task<bool> IsAuthenticatedAsync();
        Task<bool> TryLoginAsync(string email, string password);
       Task<bool> TryRegisterAsync(string username, string password, string firstname, string lastname, string function);
        Task<string> GetTokenAsync();
        bool Logout();
    }
}
