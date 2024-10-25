


namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;

public class CargoResponseDto:BaseDto{
    
    public string Destination { get; set; }
    
    public double? TotalWeight { get; set; }
   
    public List<Guid> ProductsIds { get; set; } = new List<Guid>();
    
    public List<Guid> AppUsersIds { get; set; } = new List<Guid>();
    
    public bool IsDangerous { get; set; }
    
}