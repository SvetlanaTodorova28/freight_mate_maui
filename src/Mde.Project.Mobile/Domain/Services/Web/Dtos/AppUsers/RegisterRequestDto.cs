using System.ComponentModel.DataAnnotations;
using Mde.Project.Mobile.Core.Services.Web;

namespace Mde.Project.Mobile.Core.Service.Web.Dtos.AppUsers;

public class RegisterRequestDto:LoginRequestDto{
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public Guid FunctionId { get; set; }
}