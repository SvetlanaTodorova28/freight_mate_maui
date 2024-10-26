


using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

namespace Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;

public class CargoResponseDto:BaseDto{
    
    public string Destination { get; set; }
    
    public double? TotalWeight { get; set; }
   
    public List<Guid> ProductsIds { get; set; } = new List<Guid>();
    
   public Guid UserId { get; set; }
   public AppUserResponseDto User { get; set; }
    
    public bool IsDangerous { get; set; }
    
}