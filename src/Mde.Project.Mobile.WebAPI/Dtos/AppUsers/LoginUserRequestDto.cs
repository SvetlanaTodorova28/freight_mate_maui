using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Mobile.WebAPI.Api.Dtos;

public class LoginUserRequestDto{
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    
}