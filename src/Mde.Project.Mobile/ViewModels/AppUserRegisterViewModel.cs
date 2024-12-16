using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;

namespace Mde.Project.Mobile.ViewModels;

public class AppUserRegisterViewModel: ObservableObject
{
    private readonly IAuthenticationServiceMobile _authenticationServiceMobile;
    private readonly IFunctionAccessService _functionAccessService;
    private readonly IUiService _uiService;

    public AppUserRegisterViewModel(IUiService uiService, IAuthenticationServiceMobile authServiceMobile, IFunctionAccessService functionAccessService)
    {
        _uiService = uiService;
        _authenticationServiceMobile = authServiceMobile;
        _functionAccessService = functionAccessService;
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
    public ICommand RegisterCommand { get; }

    public ICommand OnAppearingCommand => new Command(async () => await OnAppearingAsync());
    
    private async Task OnAppearingAsync()
    {
        var functions = await _functionAccessService.GetFunctionsAsync();
        if (functions.IsSuccess)
        {
            Functions = new ObservableCollection<Function>(functions.Data);
        }
        
    }
    public async Task<bool> ExecuteRegisterCommand()
    {
        if (Password != ConfirmPassword)
        {
            await _uiService.ShowSnackbarWarning("Password and confirm password do not match.");
            return false;
        }
        if (string.IsNullOrEmpty(Username))
        {
            await _uiService.ShowSnackbarWarning("Username can not be empty");
            return false;
        }
        if (!EmailValidator.IsValidEmail(Username))
        {
            await _uiService.ShowSnackbarWarning("Please enter a valid email address.");
            return false;
        }
        if (string.IsNullOrEmpty(Password))
        {
            await _uiService.ShowSnackbarWarning("Password can not be empty.");
            return false;
        }
        if (selectedFunction == null)
        {
            await _uiService.ShowSnackbarWarning("Function can not be empty.");
            return false;
        }
        

        AppUser user = new AppUser(){
            Username = Username,
            FirstName = FirstName,
            LastName = LastName,
            Password = Password,
            ConfirmPassword = ConfirmPassword,
            Function = SelectedFunction
        };
        var result = await _authenticationServiceMobile.TryRegisterAsync(user);
        if (result.IsSuccess)
        {
            return true;
        }
       
        return false;
        
    }
    

    
}

