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
    private readonly IAuthenticationServiceMobile _authenticationService;
   

    public CargoCreateViewModel(ICargoService cargoService, IUiService uiService, IAppUserService appUserService,
        IAuthenticationServiceMobile authenticationService)
    {
        _cargoService = cargoService;
        _uiService = uiService;
        _appUserService = appUserService;
        _authenticationService = authenticationService;
        LoadUsersCommand = new AsyncRelayCommand(LoadUsers);
        SaveCommand = new AsyncRelayCommand(SaveCargoAsync);
        LoadUsers().ConfigureAwait(false);
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
    private ObservableCollection<Cargo> cargos;
    
    public ObservableCollection<Cargo> Cargos
    {
        get { return cargos; }
        set
        {
            SetProperty(ref cargos, value);
        }
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
        try
        {
            if (string.IsNullOrWhiteSpace(Destination))
            {
                await _uiService.ShowSnackbarWarning("Please provide a valid destination.");
                return;
            }
        
            var cargo = SelectedCargo ?? new Cargo();
            cargo.Destination = Destination;
            cargo.TotalWeight = TotalWeight;
            cargo.IsDangerous = IsDangerous;

           
            if (SelectedUser != null)
            {
                cargo.Userid = SelectedUser.Id;
            }
            else if (SelectedCargo != null && SelectedCargo.Userid != null)
            {
                cargo.Userid = SelectedCargo.Userid;
            }
            else
            {
                await _uiService.ShowSnackbarWarning("Please select a user.");
                return;
            }
        
            var result = cargo.Id == Guid.Empty ? await _cargoService.CreateCargo(cargo) : await _cargoService.UpdateCargo(cargo);
            if (result.IsSuccess)
            {
                await _uiService.ShowSnackbarSuccessAsync("Cargo saved successfully 📦");
                await NotifyUserAsync(SelectedUser);
                await Shell.Current.GoToAsync("//CargoListPage");
            }
            else
            {
                await _uiService.ShowSnackbarWarning($"Error saving cargo: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            await _uiService.ShowSnackbarWarning("An error occurred: " + ex.Message);
        }
    }

    

   
    private async Task LoadSelectedCargoData(){
         await LoadUsers();
        PageTitle = SelectedCargo != null ? "Edit Cargo" : "Add Cargo";
        Destination = SelectedCargo?.Destination ?? string.Empty;
        TotalWeight = SelectedCargo?.TotalWeight ?? 0;
        IsDangerous = SelectedCargo?.IsDangerous ?? false;

        if (SelectedCargo != null && Users != null)
        {
            SelectedUser = Users.FirstOrDefault(u => u.Id == SelectedCargo.Userid);
        }
    }
    
    private async Task NotifyUserAsync(AppUserResponseDto user)
    {
        // Fetch logged user ID
        var userId = await _authenticationService.GetUserIdFromTokenAsync();
        
            // Get the FCM token for the selected user
            string userFcmToken = await _appUserService.GetFcmTokenAsync(user.Id.ToString());

            if (!string.IsNullOrEmpty(userFcmToken))
            {
                // Create the FCM message payload
                var message = new
                {
                    message = new
                    {
                        token = userFcmToken,
                        notification = new
                        {
                            title = "New Cargo Assignment",
                            body = $"A new cargo has been assigned to you with destination {Destination}."
                        }
                    }
                };

                // Send the notification
                await _authenticationService.SendNotificationAsync(message);
            }
            else
            {
                await _uiService.ShowSnackbarWarning("No FCM token found for the selected user.");
            }
        }
    }

    
  




    #endregion
