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

    public async Task<(bool IsSuccess, string ErrorMessage)> CreateCargo(Cargo cargo)
    {
        var cargoDto = new CargoRequestDto
        {
            Id = Guid.Empty,
            Destination = cargo.Destination,
            TotalWeight = cargo.TotalWeight,
            IsDangerous = cargo.IsDangerous,
            Products = new List<Guid>(),
            AppUserId = cargo.Userid
        };
       

        var response = await _httpClient.PostAsJsonAsync("/api/Cargos/Add", cargoDto);

        if (response.IsSuccessStatusCode)
        {
            return (true, null); 
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
        }
    }
    public async Task<(bool IsSuccess, string ErrorMessage, Guid userId)> CreateCargoWithPdf(Stream pdfStream)
    {
        try
        {
           
            string ocrResult = await _azureOcrService.ExtractTextFromPdfAsync(pdfStream);
            if (string.IsNullOrWhiteSpace(ocrResult))
            {
                return (false, "OCR did not return any results.", Guid.Empty);
            }

            
            CargoRequestDto cargoDto = await ParseExtractedTextToCargo(ocrResult);
            
            var response = await _httpClient.PostAsJsonAsync("/api/Cargos/Add", cargoDto);
            if (response.IsSuccessStatusCode)
            {
                return (true, null, cargoDto.AppUserId);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return (false, errorMessage, cargoDto.AppUserId);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Exception when creating cargo with PDF: {ex.Message}", Guid.Empty);
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
                    var userId = await _appUserService.GetUserIdByEmailAsync(emailUser);
                    cargo.AppUserId = Guid.Parse(userId);
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




    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateCargo(Cargo cargo)
    {
        var cargoDto = new CargoRequestDto
        {
            Destination = cargo.Destination,
            TotalWeight = cargo.TotalWeight,
            IsDangerous = cargo.IsDangerous,
            Id = cargo.Id,
            Products = cargo.Products?.Select(p => p.Id).ToList(),
            AppUserId = cargo.Userid // Assign the selected user's ID
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/Cargos/Update/{cargo.Id}", cargoDto);
        if (response.IsSuccessStatusCode)
        {
            return (true, string.Empty);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (false, errorMessage);
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
