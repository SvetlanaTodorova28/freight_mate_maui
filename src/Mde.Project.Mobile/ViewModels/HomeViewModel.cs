using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;

public class HomeViewModel : ObservableObject
{
    private string? username;
    private string? password;
    private string welcomeUser = "Log in te continue";

    
    private bool showLoginSection = true;
    private bool showWelcomeSection = false;
    
     public ICommand LoginCommand { get; }
   
    
    private readonly UsernameTransformer _usernameTransformer;

    public HomeViewModel(){
        _usernameTransformer = new UsernameTransformer();
        LoginCommand = new RelayCommand(PerformLogin);
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
    public bool ShowLoginSection
    {
        get => showLoginSection;
        set => SetProperty(ref showLoginSection, value);
    }
    public bool ShowWelcomeSection
    {
        get => showWelcomeSection;
        set => SetProperty(ref showWelcomeSection, value);
    }

    
    public bool IsAuthenticated() {
        var transformed= _usernameTransformer.EnsureLowercase(username);
        if (transformed == "sve@dot.com" && password == "1234"){
            UpdateWelcomeMessage();
            ShowLoginSection = false;
            ShowWelcomeSection = true;
         
            
            return true;
        }
        
        return false;
    }
    private void UpdateWelcomeMessage() {
        Welcome = $"Welcome {_usernameTransformer.ExtractUsername(username)} !";
    }
    private void PerformLogin(){
        if (IsAuthenticated()){
            ShowWelcomeSection = true;
            ShowLoginSection = false;
        }
    }

}