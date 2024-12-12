
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Claims;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.Platforms;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;
    public class SecureWebAuthenticationStorage : IAuthenticationServiceMobile
    {
       // private const string TokenKey = "token";
        private readonly HttpClient _azureHttpClient;
        private readonly HttpClient _firebaseHttpClient;
        private readonly IFunctionAccessService _functionAccessService;

        public SecureWebAuthenticationStorage(IHttpClientFactory httpClientFactory, IFunctionAccessService functionAccessService)
        {
            _functionAccessService = functionAccessService;
            _azureHttpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
            _firebaseHttpClient = httpClientFactory.CreateClient(GlobalConstants.BaseUrlFireBase);
        }

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorageHelper.GetTokenAsync();
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
            bool success = SecureStorageHelper.RemoveToken();
            return success;
        }

        public async Task<bool> TryLoginAsync(string username, string password)
        {
            HttpResponseMessage response = await _azureHttpClient.PostAsJsonAsync("/api/accounts/login", new LoginRequestDto{
                Username = username, 
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                LoginResponseDto loginResponseDto = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                await StoreToken(loginResponseDto.Token);
                return true;
            }

            return false;
        }
        
        
        public async Task<bool> TryRegisterAsync(AppUser appUser){
            Guid functionId;
            try
            {
                functionId = await _functionAccessService.GetFunctionIdAsync(appUser.Function);
            }
            catch (KeyNotFoundException ex)
            {
                return false; 
            }
            
            RegisterRequestDto registerRequestDto =  new RegisterRequestDto{
                Username = appUser.Username, 
                Password = appUser.Password,
                FirstName = appUser.FirstName,
                ConfirmPassword = appUser.ConfirmPassword,
                LastName = appUser.LastName,
                AccessLevelTypeId = functionId
            };
            var response = await _azureHttpClient.PostAsJsonAsync("/api/accounts/register", registerRequestDto);
           
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
           
            return false;
        }

        private async Task StoreToken(string newToken)
        {
            await SecureStorageHelper.SaveTokenAsync(newToken);
        }
        
        public async Task<string> GetUserIdFromTokenAsync()
        {
            var token = await SecureStorage.GetAsync("token");
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("No token found");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new InvalidOperationException("User ID claim not found in token");

            return userIdClaim.Value;
        }
        
        
        public async Task<string> GetUserFirstNameFromTokenAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("No token found");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
    
            
            var firstNameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstName");

            return firstNameClaim?.Value ?? "Unknown"; 
        }
        
        
        public async Task SendNotificationAsync(object message)
        {
           
            var accessToken = await GetAccessTokenAsync(); 
    
            _firebaseHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Send the request
            var response = await _firebaseHttpClient.PostAsJsonAsync("https://fcm.googleapis.com/v1/projects/mde-project-mobile/messages:send", message);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to send notification. Status: {response.StatusCode}, Error: {errorDetails}");
            }
        }
        
        private async Task<string> GetAccessTokenAsync()
        {
            var resourceName = "Mde.Project.Mobile.Resources.mde-project-mobile-firebase-adminsdk-vp4ii-a5f3027e90.json";
            Assembly assembly; 

#if __IOS__
            // iOS-specifieke code, zoals toegang tot AppDelegate
            assembly = IntrospectionExtensions.GetTypeInfo(typeof(AppDelegate)).Assembly;
#else
    // Voor Android en andere platforms
    assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainApplication)).Assembly;
#endif

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("The specified resource was not found.", resourceName);
                }

                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(jsonContent)
                        .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                    var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
                    return token;
                }
            }
        }








    }