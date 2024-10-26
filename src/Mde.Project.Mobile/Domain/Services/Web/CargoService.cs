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
        //api/Cargos/GetCargosByUser/
        var cargos = await _httpClient.GetFromJsonAsync<List<CargoResponseDto>>($"/api/Cargos/GetCargosByUser/{userId}");
        if (cargos.Any())
            return cargos;

        return new List<CargoResponseDto>();
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
            AppUserId = cargo.User.Id 
        };
        // public string Destination { get; set; }
        //    
        //    public double? TotalWeight { get; set; }
        //   
        //    
        //    public List<Guid>? Products { get; set; }
        //    public Guid AppUserId { get; set; }
        //    
        //    [Required(ErrorMessage = "{0} are required")]
        //    public bool IsDangerous { get; set; }

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
            Products = cargo.Products?.Select(p => p.Id).ToList(),
            AppUserId = cargo.User.Id // Assign the selected user's ID
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

    public Task<bool> Delete(Guid cargoId)
    {
        throw new NotImplementedException();
    }
}
