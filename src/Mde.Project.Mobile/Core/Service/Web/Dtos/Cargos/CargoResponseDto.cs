using WebApplication1.Dtos.Products;
using WebApplication1.Dtos.Vehicles;

namespace WebApplication1.Dtos.Cargos;

public class CargoResponseDto:BaseDto{
    public string Name { get; set; }
    
    public double TotalWeight { get; set; }
   
    public List<ProductResponseDto> Products { get; set; }
    
    
    public List<VehicleResponseDto> Vehicles { get; set; }
}