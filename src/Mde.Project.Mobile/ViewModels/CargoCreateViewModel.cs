using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.AppUsers;

namespace Mde.Project.Mobile.ViewModels;

[QueryProperty(nameof(SelectedCargo), nameof(SelectedCargo))]
public class CargoCreateViewModel : ObservableObject
{
    private readonly ICargoService _cargoService;
    private readonly IUiService _uiService;
    private readonly IAppUserService _appUserService;

    public CargoCreateViewModel(ICargoService cargoService, IUiService uiService, IAppUserService appUserService)
    {
        _cargoService = cargoService;
        _uiService = uiService;
        _appUserService = appUserService;
        LoadUsersCommand = new AsyncRelayCommand(LoadUsers);
        SaveCommand = new AsyncRelayCommand(SaveCargoAsync);
    }

    #region Bindable Properties

    private string _pageTitle = "Add Cargo";
    public string PageTitle
    {
        get => _pageTitle;
        set => SetProperty(ref _pageTitle, value);
    }

    private Cargo _selectedCargo;
    public Cargo SelectedCargo
    {
        get => _selectedCargo;
        set
        {
            SetProperty(ref _selectedCargo, value);
            LoadSelectedCargoData();
        }
    }

    private double _totalWeight;
    public double TotalWeight
    {
        get => _totalWeight;
        set => SetProperty(ref _totalWeight, value);
    }

    private bool _isDangerous;
    public bool IsDangerous
    {
        get => _isDangerous;
        set => SetProperty(ref _isDangerous, value);
    }

    private string _destination;
    public string Destination
    {
        get => _destination;
        set => SetProperty(ref _destination, value);
    }

    // Collection of available users
    private ObservableCollection<AppUserResponseDto> _users;
    public ObservableCollection<AppUserResponseDto> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    // Track the selected user
    private AppUserResponseDto _selectedUser;
    public AppUserResponseDto SelectedUser
    {
        get => _selectedUser;
        set => SetProperty(ref _selectedUser, value);
    }

    #endregion

    #region Commands
    
    public ICommand LoadUsersCommand { get; }
    public ICommand SaveCommand { get; }

    #endregion

    #region Methods

    // Load available users for selection
    private async Task LoadUsers()
    {
        var fetchedUsers = await _appUserService.GetUsersWithFunctions();
        Users = new ObservableCollection<AppUserResponseDto>(fetchedUsers);
    }
    
    public ICommand OnAppearingCommand => new Command(async () => await OnAppearingAsync());
    
    private async Task OnAppearingAsync()
    {
        await LoadUsers();
        
    }

    private async Task SaveCargoAsync()
    {
        if (string.IsNullOrWhiteSpace(Destination))
        {
            await _uiService.ShowSnackbarWarning("Please provide a valid destination.");
            return;
        }

        if (SelectedUser == null)
        {
            await _uiService.ShowSnackbarWarning("Please select a user.");
            return;
        }

       
        var appUser = new AppUser
        {
            Id = SelectedUser.Id,
            FirstName = SelectedUser.FirstName,
            Function = Enum.TryParse(typeof(Function), SelectedUser.AccessLevelType.Name, true, out var parsedFunction)
                ? (Function)parsedFunction
                : Function.Default 
        };


        
        Cargo cargo;
        if (SelectedCargo == null)
        {
            cargo = new Cargo();
        }
        else
        {
            cargo = SelectedCargo;
        }

        // Stel de waarden in voor cargo
        cargo.Destination = Destination;
        cargo.TotalWeight = TotalWeight;
        cargo.IsDangerous = IsDangerous;
        cargo.User = appUser;

        // Verwerk cargo door het door te geven aan de API-aanroep
        var result = cargo.Id == Guid.Empty ? await _cargoService.CreateCargo(cargo) : await _cargoService.UpdateCargo(cargo);
        if (result.IsSuccess)
        {
            await _uiService.ShowSnackbarSuccessAsync("Cargo saved successfully 📦");
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            await _uiService.ShowSnackbarWarning($"Error saving cargo: {result.ErrorMessage}");
        }
    }





    // Sync selected cargo data if editing
    private void LoadSelectedCargoData()
    {
        PageTitle = SelectedCargo != null ? "Edit Cargo" : "Add Cargo";
        Destination = SelectedCargo?.Destination ?? string.Empty;
        TotalWeight = SelectedCargo?.TotalWeight ?? 0;
        IsDangerous = SelectedCargo?.IsDangerous ?? false;

        if (SelectedCargo != null)
        {
            SelectedUser = Users.FirstOrDefault(u => u.Id == SelectedCargo.User.Id);
        }
    }

    #endregion
}
