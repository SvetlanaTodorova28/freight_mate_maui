



using Mde.Project.Mobile.WebAPI.Data;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Core.Services;

public class AccessLevelService:IAccessLevelService{
    private readonly ApplicationDbContext _applicationDbContext;
    public AccessLevelService(ApplicationDbContext applicationDbContext){
        _applicationDbContext = applicationDbContext;
       
       
    }
    public IQueryable<AccessLevel> GetAll(){
        return _applicationDbContext.AccessLevels;
    }
    
    public async Task<ResultModel<IEnumerable<AccessLevel>>> GetAllAsync(){
        var accessLevels = await _applicationDbContext
            .AccessLevels
            .ToListAsync();
        var resultModel = new ResultModel<IEnumerable<AccessLevel>>
        {
            Data = accessLevels
        };
        return resultModel;
    }
  
}