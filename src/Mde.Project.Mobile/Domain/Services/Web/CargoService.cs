using System.Net.Http.Json;
using System.Text.Json;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Utilities;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class CargoService : ICargoService
{
    private readonly HttpClient _httpClient;

    public CargoService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
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
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response: {responseContent}");

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
