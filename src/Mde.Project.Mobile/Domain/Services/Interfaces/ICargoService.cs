using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ICargoService{
    
   
    // Retrieves the cargos associated with the logged-in user
    Task<List<CargoResponseDto>> GetCargosForUser(Guid userId);
    Task<bool> CreateCargo(Cargo cargo);

    Task<bool> UpdateCargo(Cargo cargo);
    Task<bool> Delete(Guid cargoId);


}