using System.ComponentModel.DataAnnotations;
using Mde.Project.Mobile.WebAPI.Api.Dtos;

namespace  Mde.Project.Mobile.WebAPI.Dtos;

public class RegisterUserRequestDto:LoginUserRequestDto{
    
    
    /*[Required]
    [DataType(DataType.Password)]
    [Display(Name = " Password")]
    public string Password { get; set; }*/
    
    [Required]
    [Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string AccessLevelType { get; set; }
    
    
    
   
   
}