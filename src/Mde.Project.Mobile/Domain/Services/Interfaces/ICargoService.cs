using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ICargoService
{
  
    Task<ServiceResult<List<CargoResponseDto>>> GetCargosForUser(Guid userId);
    Task<ServiceResult<string>> CreateOrUpdateCargo(Cargo cargo, string totalWeightText);
    Task<ServiceResult<CargoCreationResultDto>> CreateCargoWithPdf(Stream stream, string fileExtension);
   // Task<ServiceResult<string>> UpdateCargo(Cargo cargo);
    Task<ServiceResult<string>> DeleteCargo(Guid cargoId);
}