namespace Mde.Project.Mobile.Domain.Models;

public class AppUser{
    
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Username { get; set; }
    public string LastName { get; set; }
    public List<Cargo> Cargos { get; set; }
    public Function Function { get; set; }
}

public enum Function {
    Admin, 
    Consignee, 
    Consignor,
    Driver
}
