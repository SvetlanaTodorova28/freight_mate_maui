
using Microsoft.AspNetCore.Identity;

namespace Mde.Project.Mobile.WebAPI.Entities;

public class AppUser:IdentityUser{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public AccessLevel? AccessLevel { get; set; }
    public Guid? AccessLevelId { get; set; }
}