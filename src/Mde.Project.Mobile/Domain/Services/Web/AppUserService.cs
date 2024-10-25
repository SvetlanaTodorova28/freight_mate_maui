using System.Net.Http.Json;
using Mde.Project.Mobile.Domain.Services.Interfaces;
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
    
        if (appUsers != null && appUsers.Any())
            return appUsers;

        return new List<AppUserResponseDto>(); 
    }

}