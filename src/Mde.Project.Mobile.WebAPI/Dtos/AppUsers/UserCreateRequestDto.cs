namespace Mde.Project.Mobile.WebAPI.Api.Dtos.Users;

public class UserCreateRequestDto{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
   
    public string Password { get; set; }
    
   
    
    public string Gender { get; set; }
  
    public string? ProfilePicture { get; set; }
    public string? ConfirmPassword { get; set; }




   
}