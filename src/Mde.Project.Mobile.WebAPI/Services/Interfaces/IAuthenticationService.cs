using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IAuthenticationService{
    public Task<ResultModel<AppUser>> RegisterUserAsync(AppUser user, string password);
    public Task<ResultModel<string>> Login(string username, string password);
}