using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Utilities;


namespace Mde.Project.Mobile.Domain.Services.Web;

public class CargoService:ICargoService{
    private readonly HttpClient _httpClient;
   

    public CargoService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(GlobalConstants.HttpClient);
    }

    public async Task<CargoResponseDto[]> GetCargosForUser(Guid userId)
    {
            //api/Cargos/GetCargosByUser/
            var cargos = await _httpClient.GetFromJsonAsync<CargoResponseDto[]>($"/api/Cargos/GetCargosByUser/{userId}");
            if (cargos.Any())
                return cargos;

            return new CargoResponseDto[0];

    }

    public Task<bool> CreateCargo(Cargo cargo){
        throw new NotImplementedException();
    }

    public Task<bool> UpdateCargo(Cargo cargo){
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid cargoId){
        throw new NotImplementedException();
    }
}