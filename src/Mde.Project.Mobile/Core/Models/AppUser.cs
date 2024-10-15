namespace Mde.Project.Mobile.Models;

public class AppUser{
    
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Username { get; set; }
    public string LastName { get; set; }
    public List<Cargo> Cargos { get; set; }
    public AccessLevelType AccessLevelType { get; set; }
}

public enum AccessLevelType {
    Basic, // Basisgebruiker, beperkte toegang
    Advanced, // Geavanceerde functies
    Admin, // Beheerderstoegang
}
