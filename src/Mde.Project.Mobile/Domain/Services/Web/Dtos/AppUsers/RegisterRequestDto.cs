using System.ComponentModel.DataAnnotations;


namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

public class RegisterRequestDto:LoginRequestDto{
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public Guid AccessLevelTypeId { get; set; }
}