using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface ICategoryService{
    IQueryable<Category> GetAll();
    Task<ResultModel<IEnumerable<Category>>> GetAllAsync();
    Task<ResultModel<Category>> GetByIdAsync(Guid id);
    Task<ResultModel<Category>> AddAsync(Category entity);
    Task<ResultModel<Category>> UpdateAsync(Category entity);
    Task<ResultModel<Category>> DeleteAsync(Category entity);
    Task<bool> DoesCategoryIdExistAsync(Guid id);
    Task<ResultModel<IEnumerable<Category>>> GetByName(string search);
    Task<bool> DoesCategoryNameExistsAsync(string search);
}