namespace Mde.Project.Mobile.WebAPI.Entities;

public class Cargo:BaseEntity { 
    
    public string Destination { get; set; }
    public double? TotalWeight { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    
    public bool IsDangerous { get; set; }
}