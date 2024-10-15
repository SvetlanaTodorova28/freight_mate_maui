namespace Mde.Project.Mobile.Core.Service.Interfaces
{
    public interface IAuthenticationServiceMobile
    {
        Task<bool> IsAuthenticatedAsync();
        Task<bool> TryLoginAsync(string email, string password);
        Task<string> GetTokenAsync();
        bool Logout();
    }
}
