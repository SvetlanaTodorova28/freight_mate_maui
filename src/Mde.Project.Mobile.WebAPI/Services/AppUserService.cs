using Mde.Project.Mobile.WebAPI.Api.Dtos;
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
  
    //get all users
    public async Task<ResultModel<IEnumerable<AppUser>>> GetUsersAsync(){
        var users = await _userManager
            .Users
            .Include(u => u.AccessLevel)
            .ToListAsync();
        if (users == null){
            return new ResultModel<IEnumerable<AppUser>>
            {
                Errors = new List<string> { "No users found." }
            };
        }
        return new ResultModel<IEnumerable<AppUser>>
        {
            Data = users
        };
    }

    public async Task<ResultModel<AppUser>> GetUserByEmailAsync(string email){
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){
            return new ResultModel<AppUser>
            {
                Errors = new List<string> { "User not found." }
            };
        }
        return new ResultModel<AppUser>{Data = user};
    }
    public async Task<ResultModel<AppUser>> GetUserByIdAsync(string id){
        var user = await _userManager.FindByIdAsync(id);
        if (user == null){
            return new ResultModel<AppUser>
            {
                Errors = new List<string> { "User not found." }
            };
        }
        return new ResultModel<AppUser>{Data = user};
    }
    public async Task<ResultModel<string>> GetUserFcmToken(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => u.FCMToken)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new ResultModel<string>
            {
                Errors = new List<string> { "User not found or no FCM token set." }
            };
        }

        return new ResultModel<string>
        {
            Data = user
        };
    }

    public async Task<ResultModel<AppUser>> UpdateUserAsync(AppUser appUser){
        if (appUser == null){
            return new ResultModel<AppUser>
            {
                Errors = new List<string> { "User to update is null." }
            };
        }
        var user = await _userManager.FindByIdAsync(appUser.Id);
        if (user == null){
            return new ResultModel<AppUser>
            {
                Errors = new List<string> { "User not found." }
            };
        }
        user.FirstName = appUser.FirstName;
        user.LastName = appUser.LastName;
        user.Email = appUser.Email;
        user.PhoneNumber = appUser.PhoneNumber;
        user.AccessLevelId = appUser.AccessLevelId;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded){
            return new ResultModel<AppUser>{
                Errors = new List<string>(updateResult.Errors.Select(x => x.Description))
            };
        }
        return new ResultModel<AppUser>{
            Data = user
        };
    }

    public async Task<ResultModel<AppUser>> DeleteUserAsync(string userId){
        var userResult = await GetUserByIdAsync(userId);
        if (!userResult.Success){
            return new ResultModel<AppUser>{
                Errors = new List<string>{
                    userResult.Errors.FirstOrDefault()?? "User not found."
                }
            };
        }
        var deleteResult = await _userManager.DeleteAsync(userResult.Data);
        if (!deleteResult.Succeeded){
            return new ResultModel<AppUser>{
                Errors = new List<string>(deleteResult.Errors.Select(x => x.Description))
            };
        }
        return new ResultModel<AppUser>{
            Data = userResult.Data
        };
    }

    public async Task<ResultModel<string>> UpdateUserPasswordAsync(string userId, string currentPassword,
        string newPassword){
        var userResult = await GetUserByIdAsync(userId);
        if (!userResult.Success){
            return new ResultModel<string>{
                Errors = new List<string>{
                    userResult.Errors.FirstOrDefault() ?? "User not found."
                }
            };
        }
        
        var passwordResult = await _userManager.ChangePasswordAsync(userResult.Data,
            currentPassword, newPassword);
        if (!passwordResult.Succeeded){
            return new ResultModel<string>{
                Errors = new List<string>(passwordResult.Errors.Select(x => x.Description))
            };
        }

        return new ResultModel<string>{
            Data = "Password updated successfully."
        };
    }
    /*public async Task<ResultModel<IEnumerable<AppUser>>> GetUsersByRoleAsync()
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
    }*/

    public async Task<ResultModel<string>> UpdateUserFcmToken(string userId, string newToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return new ResultModel<string>
            {
                Errors = new List<string> { "User not found." }
            };
        }

        user.FCMToken = newToken;
        _applicationDbContext.Update(user);
        await _applicationDbContext.SaveChangesAsync();
        return new ResultModel<string>
        {
            Data = user.FCMToken,
          
        };
    }


  


}