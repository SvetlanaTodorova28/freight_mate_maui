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
    private readonly IGeocodingService _geocodingService;

    public CargoService(IHttpClientFactory httpClientFactory,
        IOcrService azureOcrService, IAppUserService appUserService, IGeocodingService geocodingService)
    {
        _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
        _azureOcrService = azureOcrService;  
        _appUserService = appUserService;
        _geocodingService = geocodingService;
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
                return ServiceResult<List<CargoResponseDto>>.Failure("Failed to fetch cargos.");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<CargoResponseDto>>.Failure("An unexpected error occurred while fetching cargos. Please contact support if the problem persists.");
        }
    }


    public async Task<ServiceResult<string>> CreateOrUpdateCargo(Cargo cargo, string totalWeightText)
    {
        if (!double.TryParse(totalWeightText, out double parsedWeight) || parsedWeight <= 0)
        {
            return ServiceResult<string>.Failure("Weight must be a positive number.");
        }
        if (string.IsNullOrWhiteSpace(cargo.Destination))
        {
            return ServiceResult<string>.Failure("Please provide a valid destination.");
        }

        try{
            var locations = await _geocodingService.GetLocationsAsync(cargo.Destination);
            if (locations == null || !locations.Any())
            {
                return ServiceResult<string>.Failure("Location Not Found. The specified location could not be resolved.");
            }
            var location = locations?.FirstOrDefault();
            if (location != null){
                bool isValidPlacemark =
                    await GeocodingHelper.ValidateDestination(location, cargo.Destination, _geocodingService);
                if (!isValidPlacemark){
                    return ServiceResult<string>.Failure("Please provide a valid destination.");
                }
            }
        }
        catch (Exception ex){
           
            return ServiceResult<string>.Failure("Location Not Found.The specified location could not be resolved.");
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
                return ServiceResult<string>.Success("Cargo saved successfully 📦");
            }
            else
            {
                
                return ServiceResult<string>.Failure("Cargo could not be saved. Please try again.");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure("An error occurred while saving the cargo. Please contact support if the problem persists.");
        }
    }


   public async Task<ServiceResult<CargoCreationResultDto>> CreateCargoWithPdf(Stream stream, string fileExtension)
{
    if (stream == null)
    {
        return ServiceResult<CargoCreationResultDto>.Failure("Please provide a file to process.");
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
            return ServiceResult<CargoCreationResultDto>.Failure("Failed to add cargo. Please check your data and try again");
        }

        try{
            var locations = await _geocodingService.GetLocationsAsync(parsedCargoResult.Data.Destination);
            if (locations == null || !locations.Any()){
                return ServiceResult<CargoCreationResultDto>.Failure(
                    "Location Not Found. The specified location could not be resolved.");
            }

            var location = locations.FirstOrDefault();
            bool isValidPlacemark =
                await GeocodingHelper.ValidateDestination(location, parsedCargoResult.Data.Destination,
                    _geocodingService);
            if (!isValidPlacemark){
                return ServiceResult<CargoCreationResultDto>.Failure("Please provide a valid destination.");
            }

        }
        catch (Exception ex){
            return ServiceResult<CargoCreationResultDto>.Failure("Location Not Found. The specified location could not be resolved.");
        }

        var response = await _httpClient.PostAsJsonAsync("/api/Cargos/Add", parsedCargoResult.Data);
        if (!response.IsSuccessStatusCode)
        {
            return ServiceResult<CargoCreationResultDto>.Failure("Failed to add cargo. Please check your data and try again.");
        }

        var result = new CargoCreationResultDto
        {
            UserId = parsedCargoResult.Data.AppUserId,
            Destination = parsedCargoResult.Data.Destination,
            TotalWeight = parsedCargoResult.Data.TotalWeight,
            IsDangerous = parsedCargoResult.Data.IsDangerous
        };
        return ServiceResult<CargoCreationResultDto>.Success(result, "Cargo saved successfully 📦");
    }
    catch (Exception ex)
    {
        return ServiceResult<CargoCreationResultDto>.Failure($"Error creating cargo. Please contact support if the problem persists.");
    }
}

    
    
    private async Task<ServiceResult<CargoRequestDto>> ParseExtractedTextToCargo(string text)
    {
        var cargo = new CargoRequestDto();
        try
        {
            
            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Destination:", StringComparison.OrdinalIgnoreCase))
                {
                    cargo.Destination = lines[i].Substring("Destination:".Length).Trim();
                 
                }
                else if (lines[i].StartsWith("Total Weight:", StringComparison.OrdinalIgnoreCase))
                {
                    cargo.TotalWeight = ParseWeight(lines[i].Substring("Total Weight:".Length).Trim());
                }
                else if (lines[i].StartsWith("Is Dangerous:", StringComparison.OrdinalIgnoreCase))
                {
                    cargo.IsDangerous = lines[i].Substring("Is Dangerous:".Length).Trim().Equals("Yes", StringComparison.OrdinalIgnoreCase);
                }
                else if (lines[i].StartsWith("Responsible:", StringComparison.OrdinalIgnoreCase))
                {
                    var email = lines[i].Substring("Responsible:".Length).Trim();
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
            
            var locations = await _geocodingService.GetLocationsAsync(cargo.Destination);
            if (locations == null || !locations.Any()){
                return ServiceResult<CargoRequestDto>.Failure(
                    "Location Not Found. The specified location could not be resolved.");
            }

            var location = locations.FirstOrDefault();
            bool isValidPlacemark =
                await GeocodingHelper.ValidateDestination(location, cargo.Destination, _geocodingService);
            if (!isValidPlacemark){
                return ServiceResult<CargoRequestDto>.Failure("Please provide a valid destination.");
            }

            return ServiceResult<CargoRequestDto>.Success(cargo);
        }
        catch (Exception ex)
        {
            return ServiceResult<CargoRequestDto>.Failure("Error parsing cargo details. Please contact support if the problem persists.");
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
                return ServiceResult<string>.Success("Cargo deleted successfully ❌" );
            }
            else
            {
                return ServiceResult<string>.Failure("Failed to delete cargo. Please try again later.");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure("An unexpected error occurred while deleting cargo. Please contact support.");
        }
    }


}