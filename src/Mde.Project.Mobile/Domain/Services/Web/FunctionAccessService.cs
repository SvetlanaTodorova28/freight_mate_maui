using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Functions;
using Mde.Project.Mobile.Helpers;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class FunctionAccessService:IFunctionAccessService{
    private  Dictionary<Function, Guid> functionMappings ;
    private readonly HttpClient _azureHttpClient;
    
    public FunctionAccessService(IHttpClientFactory httpClientFactory)
    {
        _azureHttpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
        
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
                
            }
        }

        functionMappings = functionDictionary;
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
        var token = await SecureStorageHelper.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("No token found");

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role); 

        if (roleClaim == null)
            throw new InvalidOperationException("Role claim not found in token");

       
        return roleClaim.Value.ToLower() switch
        {
            "admin" => Function.Admin,
            "advanced" => Function.Consignee,
            "basic" => Function.Driver,
            _ => throw new InvalidOperationException("Invalid role")
        };
    }
}