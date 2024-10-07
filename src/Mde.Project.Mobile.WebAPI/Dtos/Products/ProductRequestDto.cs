using System.ComponentModel.DataAnnotations;
using Mde.Project.Mobile.WebAPI.Dtos;

namespace Mde.Project.Mobile.WebAPI.Dtos.Products;
public class ProductRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0} is required")]
    public string Name { get; set; }
    
    
    [Required(ErrorMessage = "{0} is required")]
    public Guid CategoryId { get; set; }
    
    public List<Guid>? Cargos { get; set; } = new List<Guid>();
    
}