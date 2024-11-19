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
        OnAppearingCommand = new AsyncRelayCommand(OnAppearingAsync);
        CreateOrUpdateCargoFromPdfCommand = new AsyncRelayCommand(UploadAndProcessPdfAsync);
        //LoadUsers().ConfigureAwait(false);
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
    
    private ObservableCollection<AppUserResponseDto> _users = new ObservableCollection<AppUserResponseDto>();
    public ObservableCollection<AppUserResponseDto> Users
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }
    
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
    public ICommand OnAppearingCommand { get; }
    
    public ICommand CreateOrUpdateCargoFromPdfCommand { get; }

    
    #endregion

    #region Methods
    
    private async Task LoadUsers()
    {
        var fetchedUsers = await _appUserService.GetUsersWithFunctions();
        Users = new ObservableCollection<AppUserResponseDto>(fetchedUsers);
    }
    
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

        var cargo = SelectedCargo ?? new Cargo();
        cargo.Destination = Destination;
        cargo.TotalWeight = TotalWeight;
        cargo.IsDangerous = IsDangerous;
        cargo.Userid = SelectedUser?.Id ?? Guid.Empty;

        if (cargo.Userid == Guid.Empty)
        {
            await _uiService.ShowSnackbarWarning("Please select a user.");
            return;
        }

        var result = cargo.Id == Guid.Empty ? await _cargoService.CreateCargo(cargo) : await _cargoService.UpdateCargo(cargo);
        if (result.IsSuccess)
        {
            await _uiService.ShowSnackbarSuccessAsync("Cargo saved successfully 📦");
            await NotifyUserAsync(SelectedUser.Id);
            await Shell.Current.GoToAsync("//CargoListPage");
        }
        else
        {
            await _uiService.ShowSnackbarWarning($"Error saving cargo: {result.ErrorMessage}");
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
    
    private async Task NotifyUserAsync(Guid userId)
    {
     
        string userFcmToken = await _appUserService.GetFcmTokenAsync(userId.ToString());

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
                    },
                    android = new
                    {
                        notification = new
                        {
                            channel_id = "default_channel" // Zorg ervoor dat deze overeenkomt met de ID van je notificatiekanaal
                        }
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
    private async Task UploadAndProcessPdfAsync()
    {
        Stream pdfStream = await _uiService.PickAndOpenFileAsync("application/pdf");

        if (pdfStream != null)
        {
            var (IsSuccess, ErrorMessage, userId) = await _cargoService.CreateCargoWithPdf(pdfStream);
            if (IsSuccess)
            {
                await _uiService.ShowSnackbarSuccessAsync("Cargo created from PDF successfully.");
                await NotifyUserAsync(userId);
                await Shell.Current.GoToAsync("//CargoListPage");
            }
            else
            {
                await _uiService.ShowSnackbarWarning($"Failed to create cargo from PDF: {ErrorMessage}");
            }
        }
    }
    
    #endregion
}
 
