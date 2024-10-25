using Mde.Project.Mobile.WebAPI.Data;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Services;

public class CargoService:ICargoService{
     private readonly ApplicationDbContext _applicationDbContext;
   
   
    
    public CargoService(ApplicationDbContext applicationDbContext){
        _applicationDbContext = applicationDbContext;
       
       
    }
    public IQueryable<Cargo> GetAll(){
      return _applicationDbContext.Cargos.Include(c => c.Products);
    }

    public async Task<ResultModel<IEnumerable<Cargo>>> GetAllAsync(){
        var cargos = await _applicationDbContext
            .Cargos
            .Include(c => c.AppUsers)
            .Include(c => c.Products)
            .ThenInclude(p => p.Category)
            .ToListAsync();
        var resultModel = new ResultModel<IEnumerable<Cargo>>()
        {
            Data = cargos,
        };
        return resultModel;
        
    }

    public async Task<ResultModel<IEnumerable<Cargo>>> GetCargosByIdsAsync(IEnumerable<Guid> cargosIds){
        foreach (var cargoId in cargosIds){
            if (!await DoesCargoIdExistAsync(cargoId)){
                return new ResultModel<IEnumerable<Cargo>>{
                    Errors = new List<string> { $"Cargo with id {cargoId} does not exist." }
                };
            }
        }
        var cargos = await _applicationDbContext.Cargos.Where(p => cargosIds.Contains(p.Id)).ToListAsync();
        var resultModel = new ResultModel<IEnumerable<Cargo>>{
            Data = cargos
        };
        return resultModel;
    }
    public async Task<ResultModel<Cargo>> GetByIdAsync(Guid id){
        var cargo = await _applicationDbContext
            .Cargos
            .Include(c => c.AppUsers)
            .Include(c => c.Products)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (cargo is null){
            return new ResultModel<Cargo>
            {
                Errors = new List<string> { "Cargo does not exist" }
            };

        }
        return new ResultModel<Cargo> { Data = cargo };
    }

    public async Task<ResultModel<Cargo>> AddAsync(Cargo entity){

        if (await DoesCargoIdExistAsync(entity.Id)){
            return new ResultModel<Cargo> {
                Errors = new List<string> { $"A cargo with ID {entity.Id} already exists." }
            };
        }
        
        if (!entity.Products.Any()){
            return new ResultModel<Cargo> {
                Errors = new List<string> { "Cargo must contain at least one product." }
            };
        }
        
        
        _applicationDbContext.Cargos.Add(entity);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<Cargo> { Data = entity };
    }
    
    public async Task<ResultModel<Cargo>> UpdateAsync(Cargo entity){
        
        if (!await DoesCargoIdExistAsync(entity.Id)){
            return new ResultModel<Cargo>{
                Errors = new List<string> { $"No cargo with ID {entity.Id} exists." }
            };
        }
        if (!entity.Products.Any()){
            return new ResultModel<Cargo> {
                Errors = new List<string> { "Cargo must contain at least one product." }
            };
        }
        _applicationDbContext.Cargos.Update(entity);
        await _applicationDbContext.SaveChangesAsync();

        return new ResultModel<Cargo>{
            Data = entity
        };
    }

    public async Task<ResultModel<Cargo>> DeleteAsync(Cargo entity){
        if (entity == null) {
            return new ResultModel<Cargo> {
                Errors = new List<string> { "Provided cargo is null." }
            };
        }
        _applicationDbContext.Cargos.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<Cargo>
        {
            Data = entity
        };
    }

    public async Task<ResultModel<IEnumerable<Cargo>>> GetCargosWithCategoryId(Guid id){
        
      var cargos = await _applicationDbContext
          .Cargos
          .Include(c => c.AppUsers)
          .Include(c => c.Products)
          .Where(c => c.Products.Any(p => p.CategoryId == id))
          .ToListAsync();
        
            
        if (cargos.Count == 0){
            return new ResultModel<IEnumerable<Cargo>>
            {
                Errors = new List<string> { "No cargos with this category ID exist." }
            };
        }
        
        return new ResultModel<IEnumerable<Cargo>> { Data = cargos };
    }

    public async Task<ResultModel<IEnumerable<Cargo>>> GetCargosWithUserId(Guid userId){
       
        var guidId = userId.ToString(); 
        var cargos = await _applicationDbContext
            .Cargos
            .Include(c => c.AppUsers)  // Ensure this relationship is correctly configured in the model
            .Include(c => c.Products)  // Load related products data
            .Where(c => c.AppUsers.Any(u =>u.Id.Equals(guidId))) // Assuming u.Id is a string in the database
            .ToListAsync();

        if (!cargos.Any()) {
            return new ResultModel<IEnumerable<Cargo>>{
                Errors = new List<string> { "No cargos found for this user." }
            };
        }

        if (cargos == null || !cargos.Any()) {
            return new ResultModel<IEnumerable<Cargo>>{
                Errors = new List<string> { "No cargos found for this user." }
            };
        }


        // Instead of treating no results as an error, simply return the empty list
        return new ResultModel<IEnumerable<Cargo>> {
            Data = cargos,
            Errors = cargos.Count == 0 ? new List<string> { "No cargos with this user ID exist." } : null
        };
    }


    public async Task<bool> DoesCargoIdExistAsync(Guid id){
        return await _applicationDbContext
            .Cargos
            .AnyAsync(a => a.Id.Equals(id));
    }
    
}