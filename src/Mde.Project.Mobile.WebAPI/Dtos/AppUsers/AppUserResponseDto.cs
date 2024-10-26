

using Mde.Project.Mobile.WebAPI.Dtos.Cargos;
using Mde.Project.Mobile.WebAPI.Dtos.Functions;

namespace Mde.Project.Mobile.WebAPI.Api.Dtos;

public class AppUserResponseDto{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public AccessLevelsResponseDto? AccessLevelType { get; set; }
    
    public List<CargoResponseDto>? Cargos { get; set; }
    
    
   
    
  
  
  
   
}