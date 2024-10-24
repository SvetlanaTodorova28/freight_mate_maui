using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IAppUserService{
    Task<ResultModel<AppUser>> CreateUserAsync(AppUser user, string password);
    
}