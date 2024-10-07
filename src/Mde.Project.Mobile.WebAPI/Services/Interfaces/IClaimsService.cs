using System.Security.Claims;
using Mde.Project.Mobile.WebAPI.Entities;

namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IClaimService{
    Task<IEnumerable<Claim>> GenerateClaimsForUser(AppUser user);
}