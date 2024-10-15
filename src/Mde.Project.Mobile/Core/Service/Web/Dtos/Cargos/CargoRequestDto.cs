using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Mobile.Core.Service.Web.Dto;

public class CargoRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0} is required")]
    public string Destination { get; set; }
    
    
    public double? TotalWeight { get; set; }
   
    [Required(ErrorMessage = "{0} are required")]
    public List<Guid> Products { get; set; }
    
  
}