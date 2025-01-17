using Mde.Project.Mobile.WebAPI.Api.Dtos;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Models;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IAuthenticationService{
  
    public Task<ResultModel<string>> Login(string username, string password);
    Task<ResultModel<AppUser>> RegisterUserAsync(AppUser newUser, string password);
}