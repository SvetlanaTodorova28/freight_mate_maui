

using Mde.Project.Mobile.WebAPI.Dtos.Products;

namespace Mde.Project.Mobile.WebAPI.Dtos.Cargos;

public class CargoResponseDto:BaseDto{
    public string Destination { get; set; }
    
    public double? TotalWeight { get; set; }
   
    public List<Guid> ProductsIds { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public bool IsDangerous { get; set; }
    
}