using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class CargoService : ICargoService
{
    private readonly HttpClient _httpClient;
   
    private readonly AzureOcrService _azureOcrService;

    public CargoService(IHttpClientFactory httpClientFactory,
        AzureOcrService azureOcrService)
    {
        _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
        _azureOcrService = azureOcrService;  
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
    public async Task<(bool IsSuccess, string ErrorMessage)> CreateCargoWithPdf(Stream pdfStream)
    {
        try
        {
            string extractedText = await _azureOcrService.ExtractTextFromPdfAsync(pdfStream);
            Cargo cargo = ParseExtractedTextToCargo(extractedText);  // Verwerk de tekst naar Cargo

            var cargoDto = new CargoRequestDto
            {
                Destination = cargo.Destination,
                TotalWeight = cargo.TotalWeight,
                IsDangerous = cargo.IsDangerous,
                Products = cargo.Products?.Select(p => p.Id).ToList(),
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
        catch (Exception ex)
        {
            return (false, $"Exception when creating cargo with PDF: {ex.Message}");
        }
    }

    private Cargo ParseExtractedTextToCargo(string text)
    {
        var cargo = new Cargo();
        
        var destinationPattern = new Regex(@"Destination:\s*(.*)", RegexOptions.IgnoreCase);
        var weightPattern = new Regex(@"Total weight:\s*(\d+) kg", RegexOptions.IgnoreCase);
        var dangerousPattern = new Regex(@"Dangerous:\s*(Yes|No)", RegexOptions.IgnoreCase);
        var responsiblePattern = new Regex(@"Responsible:\s*(.*)", RegexOptions.IgnoreCase);

        // Extracting Destination
        var destinationMatch = destinationPattern.Match(text);
        if (destinationMatch.Success)
        {
            cargo.Destination = destinationMatch.Groups[1].Value.Trim();
        }

        // Extracting Total Weight
        var weightMatch = weightPattern.Match(text);
        if (weightMatch.Success && int.TryParse(weightMatch.Groups[1].Value, out var weight))
        {
            cargo.TotalWeight = weight;
        }

        // Extracting IsDangerous
        var dangerousMatch = dangerousPattern.Match(text);
        if (dangerousMatch.Success)
        {
            cargo.IsDangerous = dangerousMatch.Groups[1].Value.Equals("Ja", StringComparison.OrdinalIgnoreCase);
        }
        
        // Extracting Responsible
        var responsibleMatch = responsiblePattern.Match(text);
        if (responsibleMatch.Success)
        {
            cargo.Destination = responsibleMatch.Groups[1].Value.Trim();
        }

        return cargo;
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
                return (true, string.Empty);  // Succesvol verwijderd
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to delete cargo: {error}");  // Er ging iets mis tijdens het verwijderen
            }
        }
        catch (Exception ex)
        {
            return (false, $"Exception when deleting cargo: {ex.Message}");  // Uitzondering gevangen
        }
    }

}
