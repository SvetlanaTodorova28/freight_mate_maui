using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IProductService{
    IQueryable<Product> GetAll();
    Task<ResultModel<IEnumerable<Product>>> GetProductsByIdsAsync(IEnumerable<Guid> productsIds);
    Task<ResultModel<IEnumerable<Product>>> GetAllAsync();
    Task<ResultModel<Product>> GetByIdAsync(Guid id);
    Task<ResultModel<Product>> AddAsync(Product entity);
    Task<ResultModel<Product>> UpdateAsync(Product entity);
    Task<ResultModel<Product>> DeleteAsync(Product entity);
    Task<bool> DoesProductIdExistAsync(Guid id);
}