using System.ComponentModel.DataAnnotations;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;

public class CargoRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0} is required")]
    public string Destination { get; set; }
    
    public double? TotalWeight { get; set; }
   
   
    public List<Guid>? Products { get; set; }
   
    public Guid UserId { get; set; }
    public AppUserResponseDto User { get; set; }
    
    [Required(ErrorMessage = "{0} are required")]
    public bool IsDangerous { get; set; }
    
    
}