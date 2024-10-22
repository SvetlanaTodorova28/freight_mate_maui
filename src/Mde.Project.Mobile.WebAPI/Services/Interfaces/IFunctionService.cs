using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IFunctionService{
    IQueryable<Function> GetAll();
    Task<ResultModel<IEnumerable<Function>>> GetAllAsync();
}