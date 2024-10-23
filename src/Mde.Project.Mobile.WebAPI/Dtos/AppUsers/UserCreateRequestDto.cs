using Mde.Project.Mobile.WebAPI.Dtos.Functions;

namespace Mde.Project.Mobile.WebAPI.Api.Dtos.Users;

public class UserCreateRequestDto{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public Guid AccessLevelTypeId { get; set; }
    
    




   
}