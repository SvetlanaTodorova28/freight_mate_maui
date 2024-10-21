
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Core.Service.Web.Dto;
using Mde.Project.Mobile.Core.Service.Web.Dtos.AppUsers;
using Mde.Project.Mobile.Core.Services.Web;
using Microsoft.Maui.Storage;
using Utilities;

namespace Mde.Project.Mobile.Core.Service.Web;
    public class SecureWebAuthenticationStorage : IAuthenticationServiceMobile
    {
        private const string TokenKey = "token";
        private readonly HttpClient _httpClient;
        public SecureWebAuthenticationStorage(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
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

        public async Task<bool> TryLoginAsync(string username, string password)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/accounts/login", new LoginRequestDto { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                LoginResponseDto loginResponseDto = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                await StoreToken(loginResponseDto.Token);
                return true;
            }

            return false;
        }
        
        public async Task<bool> TryRegisterAsync(string username, string password,string firstname, string lastname)
        {
            HttpResponseMessage response = await _httpClient
                .PostAsJsonAsync("api/accounts/register", 
                    new RegisterRequestDto{
                        Username = username, 
                        Password = password,
                        Email = username,
                        FirstName = firstname,
                        LastName = lastname
                    });

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

