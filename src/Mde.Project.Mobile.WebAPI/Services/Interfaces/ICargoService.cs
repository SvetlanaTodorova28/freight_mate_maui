using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface ICargoService{
    IQueryable<Cargo> GetAll();
    Task<ResultModel<IEnumerable<Cargo>>> GetAllAsync();
    Task<ResultModel<IEnumerable<Cargo>>> GetCargosByIdsAsync(IEnumerable<Guid> cargosIds);
    Task<ResultModel<Cargo>> GetByIdAsync(Guid id);
    Task<ResultModel<Cargo>> AddAsync(Cargo entity);
    Task<ResultModel<Cargo>> UpdateAsync(Cargo entity);
    Task<ResultModel<Cargo>> DeleteAsync(Cargo entity);
    Task<ResultModel<IEnumerable<Cargo>>> GetCargosWithCategoryId(Guid id);
    Task<ResultModel<IEnumerable<Cargo>>> GetCargosWithUserId(Guid id);
    Task<bool> DoesCargoIdExistAsync(Guid id);
}