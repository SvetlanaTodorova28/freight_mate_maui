using Mde.Project.Mobile.WebAPI.Data;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace Mde.Project.Mobile.WebAPI.Services;

public class AppUserService:IAppUserService{
    private readonly UserManager<AppUser> _userManager;
  

    public AppUserService(UserManager<AppUser> userManager){
        _userManager = userManager;
      
    }
    public async Task<ResultModel<AppUser>> CreateUserAsync(AppUser user, string password){
        
        var newUser = new AppUser(){
            UserName = user.UserName,
            Email = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Function = user.Function
            
        };
       
        
            
        var result = await _userManager.CreateAsync(newUser, password);

        if (!result.Succeeded){
            return new ResultModel<AppUser>{
                Errors = new List<string>(result.Errors.Select(x => x.Description))
            };
        }
       
      
        
        return new ResultModel<AppUser>{
            Data = newUser
        };
    }
}