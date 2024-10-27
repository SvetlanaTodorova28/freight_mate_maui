using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;

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

   
    
    private ObservableCollection<Function> functions;

    public ObservableCollection<Function> Functions
    {
        get  =>  functions; 
        set  => SetProperty(ref functions, value); 
    }
    
    private Function selectedFunction;

    public Function SelectedFunction{
        get => selectedFunction;
        set => SetProperty(ref selectedFunction, value);
    }

    public ICommand OnAppearingCommand => new Command(async () => await OnAppearingAsync());
    
    private async Task OnAppearingAsync()
    {
        Functions = new ObservableCollection<Function>(await authenticationServiceMobile.GetFunctionsAsync());
        
    }
    public async Task<bool> ExecuteRegisterCommand()
    {
        
        if (Password != ConfirmPassword)
        {
            await uiService.ShowSnackbarWarning("Password and confirm password do not match.");
            return false;
        }
        if (string.IsNullOrEmpty(Username))
        {
            await uiService.ShowSnackbarWarning("Username can not be empty");
            return false;
        }
        if (!IsValidEmail(Username))
        {
            await uiService.ShowSnackbarWarning("Please enter a valid email address.");
            return false;
        }
        if (string.IsNullOrEmpty(Password))
        {
            await uiService.ShowSnackbarWarning("Password can not be empty.");
            return false;
        }

        var isRegistered = await authenticationServiceMobile.TryRegisterAsync(Username, Password, ConfirmPassword, FirstName, LastName, selectedFunction);
        return isRegistered; 
    }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    
}

