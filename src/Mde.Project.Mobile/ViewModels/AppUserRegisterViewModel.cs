using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class AppUserRegisterViewModel: ObservableObject
{
    private readonly IAuthenticationServiceMobile authenticationServiceMobile;
    private readonly IUiService uiService;


    public ICommand RegisterCommand { get; }

    public AppUserRegisterViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile)
    {
        this.uiService = uiService;
        authenticationServiceMobile = authServiceMobile;
        RegisterCommand = new RelayCommand(async () => await ExecuteRegisterCommand());
        
    }

    private string username;
    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }
    private string firstName;
    public string FirstName
    {
        get => firstName;
        set => SetProperty(ref firstName, value);
    }
    private string lastName;
    public string LastName
    {
        get => lastName;
        set => SetProperty(ref lastName, value);
    }

    private string password;
    public string Password
    {
        get => password;
        set => SetProperty(ref password, value);
    }

    private string confirmPassword;
    public string ConfirmPassword
    {
        get => confirmPassword;
        set => SetProperty(ref confirmPassword, value);
    }

    private string email;
    public string Email
    {
        get => email;
        set => SetProperty(ref email, value);
    }
    
    private ObservableCollection<Function> functions;

    public ObservableCollection<Function> Functions
    {
        get { return functions; }
        set { SetProperty(ref functions, value); }
    }
    
    private string selectedFunction;

    public string SelectedFunction{
        get => selectedFunction;
        set => SetProperty(ref selectedFunction, value);
    }

    public ICommand OnAppearingCommand => new Command(async () => await OnAppearingAsync());
    
    private async Task OnAppearingAsync()
    {
        Functions = new ObservableCollection<Function>(await authenticationServiceMobile.GetFunctionsAsync());
        
    }
    private async Task ExecuteRegisterCommand()
    {
        if (Password != ConfirmPassword)
        {
            await uiService.ShowSnackbarWarning("Password and confirm password do not match.");
            return;
        }

        var isRegistered = await authenticationServiceMobile.TryRegisterAsync(Username, Password, FirstName, LastName, selectedFunction);
        if (isRegistered)
        {
            await uiService.ShowSnackbarSuccessAsync("Registration successful. You can now log in.");
            // Redirect naar de loginpagina of hoofdpagina
            Application.Current.MainPage = new NavigationPage(new WelcomePage(uiService, authenticationServiceMobile, this));
        }
        else
        {
            await uiService.ShowSnackbarWarning("Registration failed. Please try again.");
        }
    }
}

