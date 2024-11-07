
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Functions;
using Mde.Project.Mobile.Platforms;
using Microsoft.Maui.Storage;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;
    public class SecureWebAuthenticationStorage : IAuthenticationServiceMobile
    {
        private const string TokenKey = "token";
        private const string FcmTokenKey = "fcmToken";
        private readonly HttpClient _azureHttpClient;
        private readonly HttpClient _firebaseHttpClient;
        private Dictionary<Function, Guid> functionMappings;

        public SecureWebAuthenticationStorage(IHttpClientFactory httpClientFactory)
        {
            _azureHttpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
            _firebaseHttpClient = httpClientFactory.CreateClient(GlobalConstants.BaseUrlFireBase);
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
        

        public Task<IEnumerable<Function>> GetFunctionsAsync()
        {
           
            var functions = ((Function[])Enum.GetValues(typeof(Function))).AsEnumerable();
            return Task.FromResult(functions);
        }
        private async Task EnsureFunctionMappings()
        {
            if (functionMappings != null) return;

            var functionDtos = await _azureHttpClient.GetFromJsonAsync<IEnumerable<AccessLevelsResponseDto>>("/api/AccessLevels");

            Dictionary<Function, Guid> functionDictionary = new Dictionary<Function, Guid>();
            foreach (var functiondto in functionDtos)
            {
                switch (functiondto.Name.ToLower())
                {
                    case "admin":
                        functionDictionary.Add(Function.Admin,functiondto.Id);
                        break;
                    case "advanced":
                        functionDictionary.Add(Function.Consignee,functiondto.Id);
                        break;
                    case "basic":
                        functionDictionary.Add(Function.Driver,functiondto.Id);
                        break;
                    default:
                        break;
                }
            }

            functionMappings = functionDictionary;
        }
        
        public async Task<bool> TryRegisterAsync(string username, string password, string confirmPassword, string firstname, string lastname, Function function){
            Guid functionId;
            try
            {
                functionId = await GetFunctionIdAsync(function);
            }
            catch (KeyNotFoundException ex)
            {
               
                return false; 
            }
            
            RegisterRequestDto registerRequestDto =  new RegisterRequestDto{
                Username = username, 
                Password = password,
                FirstName = firstname,
                ConfirmPassword = confirmPassword,
                LastName = lastname,
                AccessLevelTypeId = functionId
            };
            var response = await _azureHttpClient.PostAsJsonAsync("/api/accounts/register", registerRequestDto);
           
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
           
            return false;
        }


        private async Task StoreToken(string token)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
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
        
        public async Task<Guid> GetFunctionIdAsync(Function function)
        {
            await EnsureFunctionMappings();

            if (functionMappings.TryGetValue(function, out Guid functionId))
            {
                return functionId; 
            }

            throw new KeyNotFoundException($"Function {function} not found in function mappings.");
        }
        
        public async Task<Function> GetUserFunctionFromTokenAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("No token found");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role); 

            if (roleClaim == null)
                throw new InvalidOperationException("Role claim not found in token");

            // Map the role claim to the corresponding Function
            return roleClaim.Value.ToLower() switch
            {
                "admin" => Function.Admin,
                "advanced" => Function.Consignee,
                "basic" => Function.Driver,
                _ => throw new InvalidOperationException("Invalid role")
            };
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
            // Fetch a fresh access token if possible (replace with your token retrieval mechanism)
            var accessToken = await GetAccessTokenAsync(); // Implement this to retrieve a valid token
    
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
            Assembly assembly; // Definieer de variabele buiten de preprocessordirectieven

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