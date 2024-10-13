using Mde.OnlineServices.MoodTracker.Core.Services.Interfaces;
using Mde.OnlineServices.MoodTracker.Core.Services.Web.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Mde.OnlineServices.MoodTracker.Core.Services.Web
{
    public class SecureWebAuthenticationStorage : IAuthenticationService
    {
        private const string TokenKey = "token";
        private readonly HttpClient _httpClient;
        public SecureWebAuthenticationStorage(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.MoodTrackerClientName);
        }

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(TokenKey);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            string encodedToken = await GetTokenAsync();

            if (encodedToken == null) { return false; }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(encodedToken) as JwtSecurityToken;

            return jsonToken.ValidTo > DateTime.UtcNow;
        }

        public bool Logout()
        {
            bool success = SecureStorage.Default.Remove(TokenKey);
            return success;
        }

        public async Task<bool> TryLoginAsync(string email, string password)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/account/login", new LoginRequestDto { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                LoginResponseDto loginResponseDto = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                await StoreToken(loginResponseDto.Token);
                return true;
            }

            return false;
        }

        private async Task StoreToken(string token)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
        }
    }
}
