namespace Mde.Project.Mobile.Domain.Models;

public class Cargo:BaseModel{
    
    public double TotalWeight{ get; set; }
    
    public bool IsDangerous{ get; set; }
    
    public string Destination { get; set; }
    
    public List<string> Products { get; set; } 
}