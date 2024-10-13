using WebApplication1.Dtos.Cargos;
using WebApplication1.Dtos.Categories;
using WebApplication1.Dtos.DangerousGoods;

namespace WebApplication1.Dtos.Products;

public class ProductResponseDto:BaseDto{
    public string Name { get; set; }
    
    public double Weight { get; set; }
    
    public decimal Price { get; set; }
    
    public CategoryResponseDto Category { get; set; }
    
    public List<CargoResponseDto>? Cargos { get; set; } = new List<CargoResponseDto>();
    
    public DangerousGoodResponseDto? DangerousGood { get; set; } 
}