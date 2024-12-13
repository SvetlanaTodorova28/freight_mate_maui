using System.Net.Http.Json;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services;

public class AppUserService:IAppUserService{
    private readonly HttpClient _httpClient;

    public AppUserService(IHttpClientFactory httpClientFactory){
        _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
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

            foreach (var user in appUsers)
            {
                if (user.AccessLevelType != null && !string.IsNullOrEmpty(user.AccessLevelType.Name))
                {
                    user.AccessLevelType.Name = MapAccessLevelToFunction(user.AccessLevelType.Name);
                }
            }

            return ServiceResult<List<AppUserResponseDto>>.Success(appUsers);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<AppUserResponseDto>>.Failure($"Error fetching users: {ex.Message}");
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
    
    public async Task<ServiceResult<bool>> StoreFcmTokenAsync(string token)
    {
        try
        {
            var authenticationStorage = MauiProgram.CreateMauiApp().Services.GetService<IAuthenticationServiceMobile>() as SecureWebAuthenticationStorage;
            var userId = await authenticationStorage?.GetUserIdFromTokenAsync();

            if (!string.IsNullOrEmpty(userId))
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/AppUsers/update-fcm-token/{userId}", token);
                if (response.IsSuccessStatusCode)
                {
                    return ServiceResult<bool>.Success(true);
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return ServiceResult<bool>.Failure($"Failed to update FCM token. Details: {errorDetails}");
                }
            }
            else
            {
                return ServiceResult<bool>.Failure("User ID not found in token.");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Unexpected error: {ex.Message}");
        }
    }


    
    public async Task<ServiceResult<string>> GetFcmTokenAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/AppUsers/get-fcm-token/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return ServiceResult<string>.Success(token);
            }
            else
            {
                return ServiceResult<string>.Failure("Failed to retrieve FCM token.");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure($"Unexpected error: {ex.Message}");
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
                if (!string.IsNullOrEmpty(userId))
                {
                    return ServiceResult<string>.Success(userId);
                }
                else
                {
                    return ServiceResult<string>.Failure("User ID not found.");
                }
            }
            else
            {
                return ServiceResult<string>.Failure("Failed to retrieve user ID from server.");
            }
        }
        catch (HttpRequestException ex)
        {
            return ServiceResult<string>.Failure($"HTTP request error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure($"Unexpected error: {ex.Message}");
        }
    }


}