



using Mde.Project.Mobile.WebAPI.Data;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Core.Services;

public class FunctionService:IAccessLevelTypeService{
    private readonly ApplicationDbContext _applicationDbContext;
    public FunctionService(ApplicationDbContext applicationDbContext){
        _applicationDbContext = applicationDbContext;
       
       
    }
    public IQueryable<Function> GetAll(){
        return _applicationDbContext.Functions;
    }
    
    public async Task<ResultModel<IEnumerable<Function>>> GetAllAsync(){
        var accessLevels = await _applicationDbContext
            .Functions
            .ToListAsync();
        var resultModel = new ResultModel<IEnumerable<Function>>
        {
            Data = accessLevels
        };
        return resultModel;
    }
  
}