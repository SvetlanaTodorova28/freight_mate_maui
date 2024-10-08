using Mde.Project.Mobile.WebAPI.Enums;
using Microsoft.AspNetCore.Identity;

namespace Mde.Project.Mobile.WebAPI.Entities;

public class AppUser:IdentityUser{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public AccessLevelType AccessLevelType { get; set; }
}