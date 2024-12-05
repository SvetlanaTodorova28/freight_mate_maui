

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;

public class CargoResponseDto:BaseDto{
    
    public string Destination { get; set; }
    
    public double? TotalWeight { get; set; }
    
    public bool IsDangerous { get; set; }
    
}