using System.ComponentModel.DataAnnotations;
using Mde.Project.Mobile.WebAPI.Dtos;


namespace Mde.Project.Mobile.WebAPI.Dtos.Cargos;

public class CargoRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0} is required")]
    public string Destination { get; set; }
    
    public double? TotalWeight { get; set; }
   
    
    public List<Guid>? Products { get; set; }
    public string AppUserId { get; set; }
    
    [Required(ErrorMessage = "{0} are required")]
    public bool IsDangerous { get; set; }
    
}