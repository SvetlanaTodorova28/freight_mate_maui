using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;
namespace Mde.Project.Mobile.ViewModels;

public class LoginViewModel : ObservableObject
{
    private string? username;
    private string? password;
    
    
    private readonly ICargoService cargoService;
    private readonly IUiService uiService;
    
    
     public ICommand LoginCommand { get; }
   
    
    private readonly UsernameTransformer _usernameTransformer;

    public LoginViewModel(ICargoService cargoService, IUiService uiService){
        _usernameTransformer = new UsernameTransformer();
        this.cargoService = cargoService;
        this.uiService = uiService;
        LoginCommand = new Command(async () => await ExecuteLoginCommand());
    
    }
    

    public string UserName
    {
        get => username;
        set{
            SetProperty(ref username, value);
           
        }

    }
    public ICargoService CargoService
    {
        get => cargoService;

    }
    public IUiService UiService
    {
        get => uiService;

    }

    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }
    
   
    private async Task ExecuteLoginCommand()
    {
        var isAuthenticated = AuthenticateUser(UserName, Password); // Implementeer deze functie
        if (isAuthenticated)
        {
            // Na succesvolle aanmelding of registratie
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//CargoListPage");

        }
        else
        {
            // Toon foutmelding
            
            
        }
    }

    private bool AuthenticateUser(string userName, string password){
        return (string.Equals(userName, "S") && string.Equals(password, "1"));
    }
    
  
    
   

}