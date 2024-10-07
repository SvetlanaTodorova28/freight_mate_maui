using System.IdentityModel.Tokens.Jwt;
using Mde.Project.Mobile.WebAPI.Entities;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface ITokenService{
    Task<JwtSecurityToken> GenerateTokenAsync(AppUser user);
}