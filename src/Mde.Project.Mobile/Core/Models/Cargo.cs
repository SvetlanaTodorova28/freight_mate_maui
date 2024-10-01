namespace Mde.Project.Mobile.Models;

public class Cargo{
    
    public Guid Id { get; set; }
    public double TotalWeight{ get; set; }
    
    public string IsDangerous{ get; set; }
    
    public string Destination { get; set; }
    
    public List<string> Products { get; set; } 
}