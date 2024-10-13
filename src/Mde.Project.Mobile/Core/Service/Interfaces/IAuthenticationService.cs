namespace Mde.OnlineServices.MoodTracker.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsAuthenticatedAsync();
        Task<bool> TryLoginAsync(string email, string password);
        Task<string> GetTokenAsync();
        bool Logout();
    }
}
