using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
using Mde.Project.Mobile.Helpers;
using Utilities;

public class AppUserService : IAppUserService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationServiceMobile _authService;
    

    public AppUserService(IHttpClientFactory httpClientFactory, IAuthenticationServiceMobile authService)
    {
        _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
        _authService = authService;
    }

    public async Task<ServiceResult<List<AppUserResponseDto>>> GetUsersWithFunctions()
    {
        try
        {
            var appUsers = await _httpClient.GetFromJsonAsync<List<AppUserResponseDto>>("/api/AppUsers/users-with-roles");

            if (appUsers == null || !appUsers.Any())
            {
                return ServiceResult<List<AppUserResponseDto>>.Failure("No users found with assigned roles.");
            }

            appUsers.ForEach(user =>
            {
                if (!string.IsNullOrEmpty(user.AccessLevelType?.Name))
                {
                    user.AccessLevelType.Name = MapAccessLevelToFunction(user.AccessLevelType.Name);
                }
            });

            return ServiceResult<List<AppUserResponseDto>>.Success(appUsers);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<AppUserResponseDto>>.Failure("An error occurred while trying to fetch user details. Please try again later.");
        }
    }

    public async Task<ServiceResult<bool>> UpdateFcmTokenOnServerAsync(string userId, string token)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/AppUsers/update-fcm-token/{userId}", token);
            return response.IsSuccessStatusCode
                ? ServiceResult<bool>.Success(true)
                : ServiceResult<bool>.Failure($"Failed to update the notification token for the user. Please verify the user ID and try again.");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure("An unexpected error occurred while updating the notification token. Please contact support if the problem persists.");
        }
    }

    public async Task<ServiceResult<string>> GetFcmTokenFromServerAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/AppUsers/get-fcm-token/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return ServiceResult<string>.Success(token);
            }
            else{
                var localToken = await SecureStorageHelper.GetFcmTokenAsync();
                if (string.IsNullOrEmpty(localToken))
                {
                    return ServiceResult<string>.Failure("Failed to retrieve the notification token from the local storage. Please login again.");
                }
                UpdateFcmTokenOnServerAsync(userId,localToken);
            }
            return ServiceResult<string>.Failure("Failed to retrieve the notification token from the server. Please check the user ID and try again.");
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure("An unexpected error occurred while retrieving the notification token. Please contact support if the problem persists.");
        }
    }

    public async Task<ServiceResult<string>> GetUserIdByEmailAsync(string email)
    {
        try
        {
            string endpoint = $"/api/AppUsers/get-user-by-email/{email}";
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var userId = await response.Content.ReadAsStringAsync();
                return !string.IsNullOrEmpty(userId)
                    ? ServiceResult<string>.Success(userId)
                    : ServiceResult<string>.Failure("No user found with the provided email address. Please verify the email and try again.");
            }
            else
            {
                return ServiceResult<string>.Failure("Failed to retrieve user ID from the server. Please ensure the email is correct and try again.");
            }
        }
        catch (HttpRequestException ex)
        {
            return ServiceResult<string>.Failure("A network error occurred while trying to fetch user details. Please check your network connection and try again.");
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure("An unexpected error occurred. Please contact support if the problem persists.");
        }
    }

    public async Task<ServiceResult<string>> GetCurrentUserIdAsync()
    {
        try
        {
            var tokenResult = await _authService.GetTokenAsync();
            if (!tokenResult.IsSuccess)
            {
                return ServiceResult<string>.Failure("Failed to retrieve authentication token. Please log in again");
            }
            
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenResult.Data);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return ServiceResult<string>.Failure("User ID not found in the authentication token. Please log in again.");
            }

            return ServiceResult<string>.Success(userIdClaim.Value);
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure("An unexpected error occurred while retrieving your user ID. Please contact support if the problem persists");
        }
    }


    private string MapAccessLevelToFunction(string accessLevelName)
    {
        return accessLevelName.ToLower() switch
        {
            "admin" => Function.Admin.ToString(),
            "advanced" => Function.Consignee.ToString(),
            "basic" => Function.Driver.ToString(),
            _ => "Unknown"
        };
    }
}
