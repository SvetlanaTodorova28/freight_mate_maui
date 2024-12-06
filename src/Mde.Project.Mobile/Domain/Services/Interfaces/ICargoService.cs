using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ICargoService{
    
   
   
    Task<List<CargoResponseDto>> GetCargosForUser(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> CreateCargo(Cargo cargo);
    Task<(bool IsSuccess, string ErrorMessage, Guid userId, string destination)> CreateCargoWithPdf(Stream stream, string fileExtension);
    Task<(bool IsSuccess, string ErrorMessage)> UpdateCargo(Cargo cargo);
    Task<(bool IsSuccess, string ErrorMessage)> DeleteCargo(Guid cargoId);




}