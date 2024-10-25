using Mde.Project.Mobile.WebAPI.Data;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Mde.Project.Mobile.WebAPI.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Services;

public class AppUserService:IAppUserService{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;
  

    public AppUserService(UserManager<AppUser> userManager, ApplicationDbContext applicationDbContext){
        _userManager = userManager;
      _applicationDbContext = applicationDbContext;
    }
    public async Task<ResultModel<AppUser>> CreateUserAsync(AppUser user, string password){
        
        var newUser = new AppUser(){
            UserName = user.UserName,
            Email = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AccessLevel = new AccessLevel(){
                Name = user.AccessLevel.Name
            }
            
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

    public async Task<ResultModel<IEnumerable<AppUser>>> GetUsersByRoleAsync()
    {
        // Gebruik de context om gebruikers op te halen en tegelijkertijd de AccessLevel te laden.
        var advancedUsers = await _applicationDbContext.Users
            .Include(u => u.AccessLevel)
            .Where(u => u.AccessLevel.Name == "advanced")
            .ToListAsync();
    
        var basicUsers = await _applicationDbContext.Users
            .Include(u => u.AccessLevel)
            .Where(u => u.AccessLevel.Name == "basic")
            .ToListAsync();

        // Combineer de lijsten met Advanced en Basic gebruikers
        var allUsers = advancedUsers.Concat(basicUsers);

        return new ResultModel<IEnumerable<AppUser>>
        {
            Data = allUsers
        };
    }


}