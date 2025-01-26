using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Claims;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
using Mde.Project.Mobile.Helpers;

using Utilities;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace Mde.Project.Mobile.Domain.Services.Web
{
    public class SecureWebAuthenticationStorage : IAuthenticationServiceMobile
    {
        private readonly HttpClient _azureHttpClient;
        private readonly HttpClient _firebaseHttpClient;
        private readonly IFunctionAccessService _functionAccessService;

        public SecureWebAuthenticationStorage(IHttpClientFactory httpClientFactory, IFunctionAccessService functionAccessService)
        {
            _functionAccessService = functionAccessService;
            _azureHttpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
            _firebaseHttpClient = httpClientFactory.CreateClient(GlobalConstants.BaseUrlFireBase);
        }

        public async Task<ServiceResult<string>> GetTokenAsync()
        {
            try
            {
                var token = await SecureStorageHelper.GetTokenAsync();
                return !string.IsNullOrEmpty(token)
                    ? ServiceResult<string>.Success(token)
                    : ServiceResult<string>.Failure("Authentication required. Please log in to continue.");
            }
            catch (Exception)
            {
                return ServiceResult<string>.Failure("Unable to access secure storage. Please try again later or contact support.");
            }
        }


        public async Task<ServiceResult<bool>> IsAuthenticatedAsync()
        {
            try
            {
                var tokenResult = await GetTokenAsync();
                if (!tokenResult.IsSuccess)
                {
                    return ServiceResult<bool>.Failure(tokenResult.ErrorMessage);
                }

                var handler = new JwtSecurityTokenHandler();
               // var jsonToken = handler.ReadToken(tokenResult.Data) as JwtSecurityToken;
               var jsonToken = handler.ReadJwtToken(tokenResult.Data);
                return jsonToken.ValidTo > DateTime.UtcNow
                    ? ServiceResult<bool>.Success(true)
                    : ServiceResult<bool>.Failure("Token has expired.");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Unexpected error while checking authentication. Please try again later or contact support.");
            }
        }

        public async Task<ServiceResult<bool>> TryLoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return ServiceResult<bool>.Failure("Username cannot be empty.");
            
            if (!EmailValidator.IsValidEmail(username))
                return ServiceResult<bool>.Failure("Please enter a valid email address.");

            if (string.IsNullOrWhiteSpace(password))
                return ServiceResult<bool>.Failure("Password cannot be empty.");
            

            try
            {
                var response = await _azureHttpClient.PostAsJsonAsync("/api/accounts/login", new LoginRequestDto
                {
                    Username = username,
                    Password = password
                });

                if (!response.IsSuccessStatusCode)
                    return ServiceResult<bool>.Failure("Invalid username or password.");

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
                    return ServiceResult<bool>.Failure("Failed to retrieve authentication token from the server.");

                await StoreToken(loginResponse.Token);
                var fcmResult = await SecureStorageHelper.GetFcmTokenAsync();
                if (string.IsNullOrEmpty(fcmResult))
                    return ServiceResult<bool>.Failure("Not FCM");

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Unexpected error during login. Please try again later or contact support.");
            }
        }


        public async Task<ServiceResult<bool>> TryRegisterAsync(AppUser appUser)
        {
           
            if (string.IsNullOrEmpty(appUser.Username))
                return ServiceResult<bool>.Failure("Username cannot be empty.");

            if (!EmailValidator.IsValidEmail(appUser.Username))
                return ServiceResult<bool>.Failure("Please enter a valid email address.");

            if (string.IsNullOrEmpty(appUser.Password))
                return ServiceResult<bool>.Failure("Password cannot be empty.");

            if (appUser.Password != appUser.ConfirmPassword)
                return ServiceResult<bool>.Failure("Password and confirm password do not match.");

            if (appUser.Function == Function.Unknown)
                return ServiceResult<bool>.Failure("Function cannot be empty.");

            try
            {
                var functionId = await _functionAccessService.GetFunctionIdAsync(appUser.Function);
                if (!functionId.IsSuccess)
                    return ServiceResult<bool>.Failure(functionId.ErrorMessage);

                var registerRequest = new RegisterRequestDto
                {
                    Username = appUser.Username,
                    Password = appUser.Password,
                    ConfirmPassword = appUser.ConfirmPassword,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    AccessLevelTypeId = functionId.Data
                };

                var response = await _azureHttpClient.PostAsJsonAsync("/api/accounts/register", registerRequest);

                return response.IsSuccessStatusCode
                    ? ServiceResult<bool>.Success(true)
                    : ServiceResult<bool>.Failure("Registration failed.");
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure("Unexpected error during registration. Please try again later or contact support.");
            }
        }


        public async Task<ServiceResult<bool>> Logout()
        {
        
            try
            {
                var success = SecureStorageHelper.RemoveToken();
            
                return success
                    ? ServiceResult<bool>.Success(true)
                    : ServiceResult<bool>.Failure("Failed to remove token.");
                
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure("Unexpected error during logout. Please try again later or contact support.");
            }
            
        }

        public async Task<ServiceResult<string>> GetUserIdFromTokenAsync()
        {
            try
            {
                var tokenResult = await GetTokenAsync();
                if (!tokenResult.IsSuccess)
                {
                    return tokenResult; 
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tokenResult.Data);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                return userIdClaim != null
                    ? ServiceResult<string>.Success(userIdClaim.Value)
                    : ServiceResult<string>.Failure("Unable to verify user identity. Please log in again.");
            }
            catch (Exception)
            {
                return ServiceResult<string>.Failure("An unexpected issue occurred while processing your authentication information. Please try again or contact support.");
            }
        }

        public async Task<ServiceResult<bool>> SendNotificationAsync(object message)
        {
            try
            {
                var accessTokenResult = await GetAccessTokenAsync();
                if (!accessTokenResult.IsSuccess)
                {
                    return ServiceResult<bool>.Failure(accessTokenResult.ErrorMessage);
                }

                _firebaseHttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessTokenResult.Data);

                var response = await _firebaseHttpClient.PostAsJsonAsync(
                    "https://fcm.googleapis.com/v1/projects/mde-project-mobile/messages:send", message);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return ServiceResult<bool>.Failure($"Failed to send notification: {errorDetails}");
                }

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Unexpected error while sending notification. Please try again later or contact support.");
            }
        }
        public async Task<ServiceResult<string>> GetUserFirstNameFromTokenAsync()
        {
            try
            {
                var tokenResult = await GetTokenAsync();
                if (!tokenResult.IsSuccess)
                {
                    return ServiceResult<string>.Failure(tokenResult.ErrorMessage);
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tokenResult.Data);

                var firstNameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "FirstName");

                return firstNameClaim != null
                    ? ServiceResult<string>.Success(firstNameClaim.Value)
                    : ServiceResult<string>.Failure("First name claim not found in token.");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure($"Unexpected error while retrieving first name. Please try again later or contact support.");
            }
        }

        private async Task<ServiceResult<string>> GetAccessTokenAsync()
        {
            try
            {
                var resourceName = "Mde.Project.Mobile.Resources.mde-project-mobile-firebase-adminsdk-vp4ii-a5f3027e90.json";
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        return ServiceResult<string>.Failure("Firebase credentials file not found.");
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        var jsonContent = reader.ReadToEnd();
                        var credential = Google.Apis.Auth.OAuth2.GoogleCredential
                            .FromJson(jsonContent)
                            .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                        var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
                        return ServiceResult<string>.Success(token);
                    }
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure($"Unexpected error retrieving access token. Please try again later or contact support.");
            }
        }

        private async Task StoreToken(string newToken)
        {
            await SecureStorageHelper.SaveTokenAsync(newToken);
        }
    }
}
