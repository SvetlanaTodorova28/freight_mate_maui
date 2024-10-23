using System.ComponentModel.DataAnnotations;
using Mde.Project.Mobile.WebAPI.Api.Dtos;
using Mde.Project.Mobile.WebAPI.Dtos.Functions;

namespace  Mde.Project.Mobile.WebAPI.Dtos;

public class RegisterUserRequestDto:LoginUserRequestDto{
    
    
    [Required]
    [Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
    
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public Guid AccessLevelTypeId { get; set; }
    
    
    
   
   
}