namespace Mde.Project.Mobile.Domain.Models;

public class AppUser{
    
    public string FirstName { get; set; }
    public string Username { get; set; }
    public string LastName { get; set; }
    public Function Function { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public enum Function {
    Admin, 
    Consignee, 
    Driver
}
