namespace Mde.Project.Mobile.WebAPI.Api.Dtos.Users;

public class UserCreateRequestDto{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string AccessLevelType { get; set; }
    
    public string? ConfirmPassword { get; set; }




   
}