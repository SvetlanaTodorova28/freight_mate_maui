using System.Security.Claims;
using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Utilities;

namespace Mde.Project.Mobile.WebAPI.Services;

public class ClaimsService:IClaimsService{
     private readonly UserManager<AppUser> _userManager;
  
    public ClaimsService(UserManager<AppUser> userManager){
        _userManager = userManager;
    }

    public async Task<IEnumerable<Claim>> GenerateClaimsForUser(AppUser user){
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        };
        //get the claims from the user table (if any)
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        
        claims.Add(new Claim("FirstName", user.FirstName ?? ""));
        claims.Add(new Claim("LastName", user.LastName ?? ""));
        claims.Add(new Claim(GlobalConstants.AdvancedAccessLevelClaimType, user.AccessLevelType.ToString()));
        
        
        //get the roles of the user from the user table (if any)
        var userRoles = await _userManager.GetRolesAsync(user);
        //add the roles of the user to the claims list of the user
        //Het opslaan van rollen als claims in het authenticatie token (zoals een JWT) elimineert de noodzaak om herhaaldelijk
        //de database te raadplegen om de rollen van een gebruiker op te halen bij elke aanvraag.
        //Dit vermindert de belasting van de database en verbetert de responsiviteit van de applicatie, vooral bij hoog verkeer.
        foreach (var userRole in userRoles){
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        
        
        return claims;
    }
}