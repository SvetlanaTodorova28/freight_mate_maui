namespace Mde.Project.Mobile.Domain.Models;

public class Cargo:BaseModel{
    
    public string Destination { get; set; }
    public double? TotalWeight { get; set; }
    public bool IsDangerous { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public Guid Userid { get; set; }
    public AppUser User { get; set; }
    
}