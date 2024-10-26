



using Mde.Project.Mobile.WebAPI.Api.Dtos;

namespace Mde.Project.Mobile.WebAPI.Dtos.Cargos;

public class CargoResponseDto:BaseDto{
    public string Destination { get; set; }
    
    public double? TotalWeight { get; set; }
   
   public List<Guid> ProductsIds { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public AppUserResponseDto AppAppUser { get; set; }
    
    public bool IsDangerous { get; set; }
    
}