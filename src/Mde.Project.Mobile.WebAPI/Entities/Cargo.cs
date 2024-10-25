namespace Mde.Project.Mobile.WebAPI.Entities;

public class Cargo:BaseEntity { 
    
    public string Destination { get; set; }
    public double? TotalWeight { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    
    public List<AppUser>? AppUsers { get; set; } = new List<AppUser>();
    
    public bool IsDangerous { get; set; }
}