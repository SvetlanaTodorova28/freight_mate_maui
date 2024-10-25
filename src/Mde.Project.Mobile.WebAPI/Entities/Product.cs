namespace Mde.Project.Mobile.WebAPI.Entities;

public class Product:BaseEntity{
    public string Name { get; set; }
    
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public List<Cargo>? Cargos { get; set; } = new List<Cargo>();
}