using System.ComponentModel.DataAnnotations;
using WebApplication1.Dtos.Products;
using WebApplication1.Dtos.Vehicles;

namespace WebApplication1.Dtos.Cargos;

public class CargoRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0} is required")]
    public string Name { get; set; }
    
    
    public double? TotalWeight { get; set; }
   
    [Required(ErrorMessage = "{0} are required")]
    public List<Guid> Products { get; set; }
    
    [Required(ErrorMessage = "{0} are required")]
    public List<Guid> Vehicles { get; set; }
}