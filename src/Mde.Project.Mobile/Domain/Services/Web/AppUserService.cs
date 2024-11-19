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
    public async Task<List<AppUserResponseDto>> GetUsersWithFunctions()
    {
        var appUsers = await _httpClient.GetFromJsonAsync<List<AppUserResponseDto>>("/api/AppUsers/users-with-roles");

        if (appUsers == null || !appUsers.Any())
            return new List<AppUserResponseDto>();
        
        foreach (var user in appUsers)
        {
            if (user.AccessLevelType != null && !string.IsNullOrEmpty(user.AccessLevelType.Name))
            {
                user.AccessLevelType.Name = MapAccessLevelToFunction(user.AccessLevelType.Name);
            }
        }

        return appUsers;
    }
    private string MapAccessLevelToFunction(string accessLevelName)
    {
        return accessLevelName.ToLower() switch
        {
            "admin" => Function.Admin.ToString(),
            "advanced" => Function.Consignee.ToString(),
            "basic" => Function.Driver.ToString(),
            _ => throw new InvalidOperationException("Invalid access level name")
        };
    }
    
    public async Task StoreFcmTokenAsync(string token){
        var authenticationStorage = MauiProgram.CreateMauiApp().Services.GetService<IAuthenticationServiceMobile>() as SecureWebAuthenticationStorage;
        var userId = await authenticationStorage?.GetUserIdFromTokenAsync();
    
        if (!string.IsNullOrEmpty(userId))
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/AppUsers/update-fcm-token/{userId}", token );
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to update FCM token.");
            }
        }
        else
        {
            throw new InvalidOperationException("User ID not found in token.");
        }
    }
    
    public async Task<string> GetFcmTokenAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"/api/AppUsers/get-fcm-token/{userId}");
        
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
        else
        {
            throw new Exception("Failed to retrieve FCM token.");
        }
    }

    public async Task<string> GetUserIdByEmailAsync(string email)
    {
        try
        {
            
            string endpoint = $"/api/AppUsers/get-user-by-email/{email}";

           
            var userId = await _httpClient.GetStringAsync(endpoint);

           
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User ID not found.");
                return null;
            }

            return userId;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Error: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserIdByEmailAsync: {ex.Message}");
            return null;
        }
    }

}