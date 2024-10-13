using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos.Products;

public class ProductRequestDto:BaseDto{
    
    [Required(ErrorMessage = "{0} is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    public double Weight { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "{0} is required")]
    public Guid CategoryId { get; set; }
    
    public List<Guid>? Cargos { get; set; } = new List<Guid>();
    
    public Guid? DangerousGoodId { get; set; } 
}