namespace Mde.Project.Mobile.Models;

public class AppUser{
    
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public List<Cargo> Cargos { get; set; }
}