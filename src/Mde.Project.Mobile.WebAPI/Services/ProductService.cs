using Mde.Project.Mobile.WebAPI.Data;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Services;

public class ProductService:IProductService{
  private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICargoService _cargoService;
    
    public ProductService(ApplicationDbContext applicationDbContext, ICargoService cargoService){
        _applicationDbContext = applicationDbContext;
        _cargoService = cargoService;
    }
    public IQueryable<Product> GetAll(){
        return _applicationDbContext
            .Products
            .Include(p => p.Category)
            .Include(p => p.Cargos);

    }
    public async Task<ResultModel<IEnumerable<Product>>> GetProductsByIdsAsync(IEnumerable<Guid> productsIds){
        foreach (var productId in productsIds){
            if (!await DoesProductIdExistAsync(productId)){
                return new ResultModel<IEnumerable<Product>>{
                    Errors = new List<string> { $"Product with id {productId} does not exist." }
                };
            }
        }
        var products = await _applicationDbContext.Products.Where(p => productsIds.Contains(p.Id)).ToListAsync();
        var resultModel = new ResultModel<IEnumerable<Product>>()
        {
            Data = products,
        };
        return resultModel;
    }
    
    public  async Task<ResultModel<IEnumerable<Product>>> GetAllAsync(){
        var products = await _applicationDbContext
            .Products
            .Include(p => p.Category)
            .Include(p => p.Cargos)
            .ToListAsync();
        var resultModel = new ResultModel<IEnumerable<Product>>()
        {
            Data =products,
        };
        return resultModel;
    }

    public async Task<ResultModel<Product>> GetByIdAsync(Guid id){
        var product = await  _applicationDbContext
            .Products
            .Include(c => c.Category)
            .Include(c => c.Cargos)
            
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product is null){
            return new ResultModel<Product>
            {
                Errors = new List<string> { "Product does not exist" }
            };
        }
        return new ResultModel<Product> { Data = product };
    }
   

    public async  Task<ResultModel<Product>> AddAsync(Product entity){

        if (await DoesProductIdExistAsync(entity.Id)){
            return new ResultModel<Product> {
                Errors = new List<string> { $"A product with ID {entity.Id} already exists." }
            };
        }

        if (await DoesProductNameExistsAsync(entity.Name)){
            return new ResultModel<Product> {
                Errors = new List<string> { $"A product with name {entity.Name} already exists." }
            };
        }
        if (!await DoesCategoryIdExistAsync(entity.CategoryId)){
            return new ResultModel<Product> {
                Errors = new List<string> { $"No category with ID {entity.CategoryId} exists." }
            };
        }
       
        //don't must to check if cargos exist, it's done in cargo service
        var cargosIds = entity.Cargos.Select(c => c.Id);
        var resultCargos = await _cargoService
            .GetCargosByIdsAsync(cargosIds);
        if (!resultCargos.Success){
            return new ResultModel<Product>{
                Errors = resultCargos.Errors
            };
        }
   
        _applicationDbContext.Products.Add(entity);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<Product> { Data = entity };
    }

    public async Task<ResultModel<Product>> UpdateAsync(Product entity){
       

        if (!await DoesProductIdExistAsync(entity.Id)){
            return new ResultModel<Product> {
                Errors = new List<string> { $"No product with ID {entity.Id}  exists." }
            };
        }
      
        if (!await DoesCategoryIdExistAsync(entity.CategoryId)){
            return new ResultModel<Product> {
                Errors = new List<string> { $"No category with ID {entity.CategoryId} exists." }
            };
        }
   
        _applicationDbContext.Products.Update(entity);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<Product> { Data = entity };
    }

    public async Task<ResultModel<Product>> DeleteAsync(Product entity){
        if (entity == null) {
            return new ResultModel<Product> {
                Errors = new List<string> { "Provided product is null." }
            };
        }
        //TO DO: check if product is associated with any cargo
        _applicationDbContext.Products.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<Product>
        {
            Data = entity
        };
    }

    public async Task<bool> DoesProductIdExistAsync(Guid id){
        return await _applicationDbContext
            .Products
            .AnyAsync(p => p.Id.Equals(id));
    }
    
    public async Task<bool> DoesCategoryIdExistAsync(Guid id){
        return await _applicationDbContext
            .Categories
            .AnyAsync(c => c.Id.Equals(id));
    }
   
    public async Task<bool> DoesProductNameExistsAsync(string search){
        return await _applicationDbContext
            .Products
            .AnyAsync(p => p.Name.ToLower().Contains(search.ToLower()));
    }
}