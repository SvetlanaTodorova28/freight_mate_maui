using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Functions;
using Mde.Project.Mobile.Helpers;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class FunctionAccessService : IFunctionAccessService
{
    private Dictionary<Function, Guid> _functionMappings;
    private readonly HttpClient _azureHttpClient;

    public FunctionAccessService(IHttpClientFactory httpClientFactory)
    {
        _azureHttpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
    }

    public async Task<ServiceResult<IEnumerable<Function>>> GetFunctionsAsync()
    {
        try
        {
            var functions = ((Function[])Enum.GetValues(typeof(Function)))
                .Where(f => f != Function.Unknown)
                .AsEnumerable();
            return ServiceResult<IEnumerable<Function>>.Success(functions);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<Function>>.Failure($"Error retrieving functions: {ex.Message}");
        }
    }

    private async Task<ServiceResult<bool>> EnsureFunctionMappingsAsync()
    {
        try
        {
            if (_functionMappings != null)
            {
                return ServiceResult<bool>.Success(true);
            }

            var functionDtos = await _azureHttpClient.GetFromJsonAsync<IEnumerable<AccessLevelsResponseDto>>("/api/AccessLevels");

            if (functionDtos == null || !functionDtos.Any())
            {
                return ServiceResult<bool>.Failure("Failed to retrieve access levels from the server.");
            }

            _functionMappings = functionDtos
                .Select(dto => new { Function = MapFunction(dto.Name), dto.Id })
                .Where(item => item.Function.HasValue) 
                .ToDictionary(
                    item => item.Function.Value,
                    item => item.Id
                );

            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Error ensuring function mappings: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Guid>> GetFunctionIdAsync(Function function)
    {
        var mappingResult = await EnsureFunctionMappingsAsync();
        if (!mappingResult.IsSuccess)
        {
            return ServiceResult<Guid>.Failure(mappingResult.ErrorMessage);
        }

        if (_functionMappings.TryGetValue(function, out Guid functionId))
        {
            return ServiceResult<Guid>.Success(functionId);
        }

        return ServiceResult<Guid>.Failure($"Function {function} not found in mappings.");
    }

    public async Task<ServiceResult<Function>> GetUserFunctionFromTokenAsync()
    {
        try
        {
            var token = await SecureStorageHelper.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
                return ServiceResult<Function>.Failure("No token found in secure storage.");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (roleClaim == null)
                return ServiceResult<Function>.Failure("Role claim not found in token.");

            var function = MapFunction(roleClaim.Value);

            return function != null
                ? ServiceResult<Function>.Success(function.Value)
                : ServiceResult<Function>.Failure("Invalid role in token.");
        }
        catch (Exception ex)
        {
            return ServiceResult<Function>.Failure($"Error retrieving user function: {ex.Message}");
        }
    }

    private Function? MapFunction(string roleName)
    {
        return roleName.ToLower() switch
        {
            "admin" => Function.Admin,
            "advanced" => Function.Consignee,
            "basic" => Function.Driver,
            _ => null
        };
    }
}
