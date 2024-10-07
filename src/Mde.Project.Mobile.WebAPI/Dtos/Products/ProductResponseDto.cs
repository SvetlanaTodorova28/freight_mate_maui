
using Mde.Project.Mobile.WebAPI.Dtos.Cargos;
using Mde.Project.Mobile.WebAPI.Dtos.Categories;

namespace Mde.Project.Mobile.WebAPI.Dtos.Products;

public class ProductResponseDto:BaseDto{
    public string Name { get; set; }
    
    public double Weight { get; set; }
    
    public decimal Price { get; set; }
    
    public CategoryResponseDto Category { get; set; }
    
  
}