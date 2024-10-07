using Mde.Project.Mobile.WebAPI.Data;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Services;

public class CategoryService:ICategoryService{
      private readonly ApplicationDbContext _applicationDbContext;
    
    public CategoryService(ApplicationDbContext applicationDbContext){
        _applicationDbContext = applicationDbContext;
    }
    public IQueryable<Category> GetAll(){
        return _applicationDbContext
            .Categories;
    }

    public async Task<ResultModel<IEnumerable<Category>>> GetAllAsync(){
        var categories = await _applicationDbContext
            .Categories
            .ToListAsync();
        var resultModel = new ResultModel<IEnumerable<Category>>
        {
            Data = categories
        };
        return resultModel;
    }

    public async Task<ResultModel<Category>> GetByIdAsync(Guid id){
      var category = await _applicationDbContext
          .Categories
          .FirstOrDefaultAsync(c => c.Id == id);
      if (category == null){
          return new ResultModel<Category>
          {
              Errors = new List<string> { "Category does not exist" }
          };
      }
      return new ResultModel<Category> { Data = category };
    }

    public async Task<ResultModel<Category>> AddAsync(Category entity){
        if (await DoesCategoryIdExistAsync(entity.Id)){
            return new ResultModel<Category> {
                Errors = new List<string> { $"A category with ID {entity.Id} already exists." }
            };
        }
        if (await DoesCategoryNameExistsAsync(entity.Name)){
            return new ResultModel<Category> {
                Errors = new List<string> { $"A category with name {entity.Name} already exists." }
            };
        }
        _applicationDbContext.Categories.Add(entity);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<Category> { Data = entity };

    }

    public async Task<ResultModel<Category>> UpdateAsync(Category entity){
        
        if (!await DoesCategoryIdExistAsync(entity.Id)){
            return new ResultModel<Category>
            {
                Errors = new List<string> { $"No Category with ID {entity.Id} exists." }
            };
        }
        _applicationDbContext.Categories.Update(entity);
        await _applicationDbContext.SaveChangesAsync();

        return new ResultModel<Category>{
            Data = entity
        };
    }

    public async Task<ResultModel<Category>> DeleteAsync(Category entity){
        if (entity == null) {
            return new ResultModel<Category> {
                Errors = new List<string> { "Provided category is null." }
            };
        }
        // Check if any products are associated with this category
        _applicationDbContext.Categories.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<Category>
        {
            Data = entity
        };
    }

    public async  Task<bool> DoesCategoryIdExistAsync(Guid id){
        return await _applicationDbContext.Categories.AnyAsync(a => a.Id.Equals(id));
    }

    public async Task<ResultModel<IEnumerable<Category>>> GetByName(string search){
      var category = await _applicationDbContext
          .Categories
          .Where(c => c.Name.Contains(search))
          .ToListAsync();
      if (category.Count == 0){
          return new ResultModel<IEnumerable<Category>>
          {
              Errors = new List<string> { "No category with this name exist." }
          };
      }
      return new ResultModel<IEnumerable<Category>> { Data = category };
    }
    
    public async Task<bool> DoesCategoryNameExistsAsync(string search){
        return await _applicationDbContext
            .Categories
            .AnyAsync(c => c.Name.ToLower().Contains(search.ToLower()));
    }
}