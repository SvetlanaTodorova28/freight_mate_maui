namespace Mde.Project.Mobile.Domain.Models;

public class Cargo:BaseModel{
    
    public string Destination { get; set; }
    public double? TotalWeight { get; set; }
    public bool IsDangerous { get; set; }
    public Guid Userid { get; set; }
   
    
}