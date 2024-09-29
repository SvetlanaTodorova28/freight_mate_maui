using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;

public class HomeViewModel : ObservableObject
{
    private string username;
    private string password;
    private string welcomeUser = "Log in te continue"; 
   
    
    private readonly UsernameTransformer _usernameTransformer;

    public HomeViewModel(){
        _usernameTransformer = new UsernameTransformer();
    }

    public string UserName
    {
        get => username;
        set{
            SetProperty(ref username, value);
           
        }

    }
    public string Welcome
    {
        get => welcomeUser;
        private set => SetProperty(ref welcomeUser, value); // Maak setter private
    }

    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }

    
    public bool IsAuthenticated() {
        var transformed= _usernameTransformer.EnsureLowercase(username);
        if (transformed == "sve@dot.com" && password == "1234"){
            UpdateWelcomeMessage();
            return true;
        }
        
        return false;
    }
    private void UpdateWelcomeMessage() {
        Welcome = $"Welcome {_usernameTransformer.ExtractUsername(username)} !";
    }
}