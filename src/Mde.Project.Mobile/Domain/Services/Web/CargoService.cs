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
            Id = Guid.Empty, // Typically, the ID should be set by the backend if creating a new cargo
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
                // Optionally retrieve the ID or any other data of the newly created cargo from the response
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
            // Handle exceptions that occur during the HTTP request
            return ServiceResult<string>.Failure($"An error occurred while creating the cargo: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage, Guid userId, string destination)> CreateCargoWithPdf(Stream stream, string fileExtension)
    {
        try
        {
           
            string ocrResult = fileExtension.Equals("pdf")?
                await _azureOcrService.ExtractTextFromPdfAsync(stream) : 
                await _azureOcrService.ExtractTextFromImageAsync(stream);
            if (string.IsNullOrWhiteSpace(ocrResult))
            {
                return (false, "OCR did not return any results.", Guid.Empty, string.Empty);
            }

           
            CargoRequestDto cargoDto = await ParseExtractedTextToCargo(ocrResult);
            
            var response = await _httpClient.PostAsJsonAsync("/api/Cargos/Add", cargoDto);
            if (response.IsSuccessStatusCode)
            {
                return (true, null, cargoDto.AppUserId, cargoDto.Destination);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return (false, errorMessage, Guid.Empty, string.Empty);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Exception when creating cargo with PDF: {ex.Message}", Guid.Empty, string.Empty);
        }
    }
    
    private async Task<CargoRequestDto> ParseExtractedTextToCargo(string text)
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
                    var emailUser = line.Substring("Responsible:".Length).Trim();
                    
                    var result = await _appUserService.GetUserIdByEmailAsync(emailUser);
                    if (result.IsSuccess)
                    {
                        cargo.AppUserId = Guid.Parse(result.Data);
                    }
                }
                
            }

            return cargo;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while parsing text to Cargo: {ex.Message}");
            return null;
        }
    }

    private double ParseWeight(string weightText)
    {
        var weight = weightText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (weight.Length > 0 && double.TryParse(weight[0], out var result))
        {
            return result;
        }
        return 0;
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

    public async Task<(bool IsSuccess, string ErrorMessage)> DeleteCargo(Guid cargoId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/Cargos/Delete/{cargoId}");
            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);  
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to delete cargo: {error}");  
            }
        }
        catch (Exception ex)
        {
            return (false, $"Exception when deleting cargo: {ex.Message}");  
        }
    }

}
