namespace Mde.Project.Mobile.Models;

public class UsernameTransformer{
    public string EnsureLowercase(string username){
        return username.Trim().ToLower();
    }
    public string ExtractUsername(string email)
    {
        if (string.IsNullOrEmpty(email))
            return "";

        var parts = email.Split('@');
        if (parts.Length > 0)
            return parts[0];

        return "";
    }
    

 
}