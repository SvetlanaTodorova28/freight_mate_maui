using System.Net.Http.Json;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class CargoService : ICargoService
{
    private readonly HttpClient _httpClient;
    private readonly IOcrService _azureOcrService;
    private readonly IAppUserService _appUserService;

    public CargoService(IHttpClientFactory httpClientFactory,
        IOcrService azureOcrService, IAppUserService appUserService)
    {
        _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
        _azureOcrService = azureOcrService;  
        _appUserService = appUserService;
    }

    public async Task<List<CargoResponseDto>> GetCargosForUser(Guid userId){
        var cargos = new List<CargoResponseDto>();
         cargos = await _httpClient.GetFromJsonAsync<List<CargoResponseDto>>($"/api/Cargos/GetCargosByUser/{userId}");
        return cargos;
    }

    public async Task<ServiceResult<string>> CreateCargo(Cargo cargo)
    {
        var cargoDto = new CargoRequestDto
        {
            Id = Guid.Empty,
            Destination = cargo.Destination,
            TotalWeight = cargo.TotalWeight,
            IsDangerous = cargo.IsDangerous,
            AppUserId = cargo.Userid
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Cargos/Add", cargoDto);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<string>.Success("Cargo created successfully");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return ServiceResult<string>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure($"An error occurred while creating the cargo: {ex.Message}");
        }
    }

    public async Task<ServiceResult<CargoCreationResultDto>> CreateCargoWithPdf(Stream stream, string fileExtension)
    {
        try
        {
            ServiceResult<string> ocrResult = fileExtension.Equals("pdf") ?
                await _azureOcrService.ExtractTextFromPdfAsync(stream) : 
                await _azureOcrService.ExtractTextFromImageAsync(stream);

            if (!ocrResult.IsSuccess)
            {
                return ServiceResult<CargoCreationResultDto>.Failure(ocrResult.ErrorMessage);
            }

            ServiceResult<CargoRequestDto> parsedCargoResult = await ParseExtractedTextToCargo(ocrResult.Data);
            if (!parsedCargoResult.IsSuccess)
            {
                return ServiceResult<CargoCreationResultDto>.Failure(parsedCargoResult.ErrorMessage);
            }

            CargoRequestDto cargoDto = parsedCargoResult.Data;
            var response = await _httpClient.PostAsJsonAsync("/api/Cargos/Add", cargoDto);
            if (response.IsSuccessStatusCode)
            {
                var result = new CargoCreationResultDto()
                {
                    UserId = cargoDto.AppUserId,
                    Destination = cargoDto.Destination
                };
                return ServiceResult<CargoCreationResultDto>.Success(result);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return ServiceResult<CargoCreationResultDto>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<CargoCreationResultDto>.Failure($"Exception when creating cargo with PDF: {ex.Message}");
        }
    }


    
   private async Task<ServiceResult<CargoRequestDto>> ParseExtractedTextToCargo(string text)
{
    var cargo = new CargoRequestDto();

    try
    {
        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (line.StartsWith("Destination:", StringComparison.OrdinalIgnoreCase))
            {
                cargo.Destination = line.Substring("Destination:".Length).Trim();
            }
            else if (line.StartsWith("Total Weight:", StringComparison.OrdinalIgnoreCase))
            {
                if (double.TryParse(line.Substring("Total Weight:".Length).Trim(), out double weight))
                {
                    cargo.TotalWeight = weight;
                }
                else
                {
                    return ServiceResult<CargoRequestDto>.Failure("Invalid total weight value.");
                }
            }
            else if (line.StartsWith("Is Dangerous:", StringComparison.OrdinalIgnoreCase))
            {
                cargo.IsDangerous = line.Substring("Is Dangerous:".Length).Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase);
            }
            else if (line.StartsWith("Responsible:", StringComparison.OrdinalIgnoreCase))
            {
                var emailUser = line.Substring("Responsible:".Length).Trim();
                var result = await _appUserService.GetUserIdByEmailAsync(emailUser);
                if (result.IsSuccess)
                {
                    cargo.AppUserId = Guid.Parse(result.Data);
                }
                else
                {
                    return ServiceResult<CargoRequestDto>.Failure("Responsible user ID could not be retrieved or parsed.");
                }
            }
        }

        return ServiceResult<CargoRequestDto>.Success(cargo);
    }
    catch (Exception ex)
    {
        return ServiceResult<CargoRequestDto>.Failure($"Error while parsing text to Cargo: {ex.Message}");
    }
}
   

    public async Task<ServiceResult<string>> UpdateCargo(Cargo cargo)
    {
        var cargoDto = new CargoRequestDto
        {
            Destination = cargo.Destination,
            TotalWeight = cargo.TotalWeight,
            IsDangerous = cargo.IsDangerous,
            Id = cargo.Id,
            AppUserId = cargo.Userid 
        };

        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Cargos/Update/{cargo.Id}", cargoDto);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<string>.Success("Cargo updated successfully");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return ServiceResult<string>.Failure(errorMessage);
            }
        }catch (Exception ex)
        {
            return ServiceResult<string>.Failure($"An error occurred while updating the cargo: {ex.Message}");
        }
    }

    public async Task<ServiceResult<string>> DeleteCargo(Guid cargoId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/Cargos/Delete/{cargoId}");
            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<string>.Success("Cargo successfully deleted.");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return ServiceResult<string>.Failure($"Failed to delete cargo: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure($"Exception when deleting cargo: {ex.Message}");
        }
    }


}
