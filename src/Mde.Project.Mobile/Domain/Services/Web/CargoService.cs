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

    public async Task<ServiceResult<List<CargoResponseDto>>> GetCargosForUser(Guid userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/Cargos/GetCargosByUser/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var cargos = await response.Content.ReadFromJsonAsync<List<CargoResponseDto>>();
                return cargos != null 
                    ? ServiceResult<List<CargoResponseDto>>.Success(cargos)
                    : ServiceResult<List<CargoResponseDto>>.Failure("No cargos found for the specified user.");
            }
            else
            {
                return ServiceResult<List<CargoResponseDto>>.Failure($"Failed to fetch cargos. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<CargoResponseDto>>.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }


    public async Task<ServiceResult<string>> CreateOrUpdateCargo(Cargo cargo, string totalWeightText)
    {
        if (!double.TryParse(totalWeightText, out double parsedWeight) || parsedWeight <= 0)
        {
            return ServiceResult<string>.Failure("Total weight must be a positive number.");
        }
        if (string.IsNullOrWhiteSpace(cargo.Destination))
        {
            return ServiceResult<string>.Failure("Please provide a valid destination.");
        }
        
        cargo.TotalWeight = parsedWeight;

        if (cargo.Userid == Guid.Empty)
        {
            return ServiceResult<string>.Failure("Please select a user.");
        }

        CargoRequestDto cargoDto = new CargoRequestDto
        {
            Id = cargo.Id,
            Destination = cargo.Destination,
            TotalWeight = cargo.TotalWeight,
            IsDangerous = cargo.IsDangerous,
            AppUserId = cargo.Userid
        };

        try
        {
            HttpResponseMessage response = cargo.Id == Guid.Empty
                ? await _httpClient.PostAsJsonAsync("/api/Cargos/Add", cargoDto)
                : await _httpClient.PutAsJsonAsync($"/api/Cargos/Update/{cargo.Id}", cargoDto);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<string>.Success("Cargo saved successfully.");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return ServiceResult<string>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure($"An error occurred while saving the cargo: {ex.Message}");
        }
    }


    public async Task<ServiceResult<CargoCreationResultDto>> CreateCargoWithPdf(Stream stream, string fileExtension)
    {
        if (stream == null)
        {
            return ServiceResult<CargoCreationResultDto>.Failure("No file was provided.");
        }

        try
        {
            ServiceResult<string> ocrResult = fileExtension.Equals("pdf") 
                ? await _azureOcrService.ExtractTextFromPdfAsync(stream)
                : await _azureOcrService.ExtractTextFromImageAsync(stream);

            if (!ocrResult.IsSuccess)
            {
                return ServiceResult<CargoCreationResultDto>.Failure(ocrResult.ErrorMessage);
            }

            ServiceResult<CargoRequestDto> parsedCargoResult = await ParseExtractedTextToCargo(ocrResult.Data);

            if (!parsedCargoResult.IsSuccess)
            {
                return ServiceResult<CargoCreationResultDto>.Failure(parsedCargoResult.ErrorMessage);
            }

            var response = await _httpClient.PostAsJsonAsync("/api/Cargos/Add", parsedCargoResult.Data);

            if (response.IsSuccessStatusCode)
            {
                var result = new CargoCreationResultDto
                {
                    UserId = parsedCargoResult.Data.AppUserId,
                    Destination = parsedCargoResult.Data.Destination
                };
                return ServiceResult<CargoCreationResultDto>.Success(result);
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return ServiceResult<CargoCreationResultDto>.Failure(errorMessage);
        }
        catch (Exception ex)
        {
            return ServiceResult<CargoCreationResultDto>.Failure($"Error creating cargo: {ex.Message}");
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
                    cargo.TotalWeight = ParseWeight(line.Substring("Total Weight:".Length).Trim());
                }
                else if (line.StartsWith("Is Dangerous:", StringComparison.OrdinalIgnoreCase))
                {
                    cargo.IsDangerous = line.Substring("Is Dangerous:".Length).Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase);
                }
                else if (line.StartsWith("Responsible:", StringComparison.OrdinalIgnoreCase))
                {
                    var email = line.Substring("Responsible:".Length).Trim();
                    var result = await _appUserService.GetUserIdByEmailAsync(email).ConfigureAwait(false);
                    if (result.IsSuccess)
                    {
                        cargo.AppUserId = Guid.Parse(result.Data);
                    }
                    else
                    {
                    
                        return ServiceResult<CargoRequestDto>.Failure("Failed to find a user with the given email.");
                    }
                }
            }

            if (string.IsNullOrEmpty(cargo.Destination) || cargo.AppUserId == Guid.Empty)
            {
                return ServiceResult<CargoRequestDto>.Failure("Necessary cargo information is missing.");
            }

            return ServiceResult<CargoRequestDto>.Success(cargo);
        }
        catch (Exception ex)
        {
            return ServiceResult<CargoRequestDto>.Failure($"Error parsing cargo details: {ex.Message}");
        }
    }
  

    private double ParseWeight(string weightText)
    {
        var weightParts = weightText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (weightParts.Length > 0 && double.TryParse(weightParts[0], out var weight))
        {
            return weight; 
        }
        return 0; 
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