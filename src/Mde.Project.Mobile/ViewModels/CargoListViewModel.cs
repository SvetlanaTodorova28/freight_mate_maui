using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Helpers;
using Mde.Project.Mobile.Pages;
#if ANDROID || IOS
using Mde.Project.Mobile.Platforms;
#endif
namespace Mde.Project.Mobile.ViewModels;

public class CargoListViewModel : ObservableObject
{
    private readonly ICargoService _cargoService;
    private readonly IAuthenticationServiceMobile _authenticationService;
    private readonly IFunctionAccessService _functionAccessService;
    private readonly IUiService _uiService;
    public event Action RequestAnimationUpdate;

    

    public CargoListViewModel(ICargoService cargoService, IUiService uiService, IAuthenticationServiceMobile authenticationService, IFunctionAccessService functionAccessService
    )
    {
        _cargoService = cargoService;
        _uiService = uiService;
        _authenticationService = authenticationService;
        _functionAccessService = functionAccessService;
        

        RefreshListCommand = new AsyncRelayCommand(RefreshListAsync);
        CreateCargoCommand = new AsyncRelayCommand(NavigateToCreateCargoAsync);
        EditCargoCommand = new AsyncRelayCommand<Cargo>(NavigateToEditCargoAsync);
        DeleteCargoCommand = new AsyncRelayCommand<Cargo>(DeleteCargoAsync);
        DetailsCargoCommand = new AsyncRelayCommand<Cargo>(NavigateToDetailsCargoAsync);
        PerformSearchCommand = new AsyncRelayCommand<string>(PerformSearchAsync);
        TextChangedCommand = new AsyncRelayCommand<string>(OnSearchTextChanged);
        
        UpdateSnowVisibility();
        LoadUserFunction();
        LoadUserFirstName();
        InitializeSubscriptionsCargoListUpdatedInApp();
        InitializeSubscriptionsCargoListUpdatedRemotely();
       // InitializeSubscriptionSnow();
        UpdateSnowVisibility();
        MessagingCenter.Subscribe<SettingsViewModel, bool>(this, "SnowToggleChanged", (sender, isEnabled) =>
        {
            UpdateSnowVisibility();
        });


    }

    #region Bindings
    private bool _snowVisibility;
    public bool SnowVisibility
    {
        get => _snowVisibility;
        private set => SetProperty(ref _snowVisibility, value);
    }
    

    private ObservableCollection<Cargo> _cargos = new();
    public ObservableCollection<Cargo> Cargos
    {
        get => _cargos;
        set => SetProperty(ref _cargos, value);
    }

    private Function _userFunction;
    public Function UserFunction
    {
        get => _userFunction;
        set => SetProperty(ref _userFunction, value);
    }

    private string _userFirstName;
    public string UserFirstName
    {
        get => _userFirstName;
        set => SetProperty(ref _userFirstName, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _showAnimation;
    public bool ShowAnimation
    {
        get => _showAnimation;
        set => SetProperty(ref _showAnimation, value);
    }

    #endregion

    #region Commands

    public ICommand RefreshListCommand { get; }
    public ICommand CreateCargoCommand { get; }
    public ICommand EditCargoCommand { get; }
    public ICommand DeleteCargoCommand { get; }
    public ICommand DetailsCargoCommand { get; }
    public ICommand PerformSearchCommand { get; }
    public ICommand TextChangedCommand { get; }

    #endregion

    #region Methods
  

    public event EventHandler CargosLoaded;

    private async Task RefreshListAsync()
    {
        try
        {
            IsLoading = true;
            var userIdResult = await _authenticationService.GetUserIdFromTokenAsync();
            if (!userIdResult.IsSuccess)
            {
                await _uiService.ShowSnackbarWarning(userIdResult.ErrorMessage);
                return;
            }

            var dtoCargosResult = await _cargoService.GetCargosForUser(Guid.Parse(userIdResult.Data));
            if (!dtoCargosResult.IsSuccess)
            {
                await _uiService.ShowSnackbarWarning(dtoCargosResult.ErrorMessage);
                return;
            }

            var modelCargos = dtoCargosResult.Data.Select(dto => new Cargo
            {
                Id = dto.Id,
                Destination = dto.Destination,
                IsDangerous = dto.IsDangerous,
                TotalWeight = dto.TotalWeight ?? 0,
                Userid = Guid.Parse(userIdResult.Data)
            }).ToList();

            Cargos = new ObservableCollection<Cargo>(modelCargos);
            CargosLoaded?.Invoke(this, EventArgs.Empty);
            ShowAnimation = false;
        }
        catch (Exception ex)
        {
            Cargos = new ObservableCollection<Cargo>();
            await _uiService.ShowSnackbarWarning($"Error refreshing cargo list. ");
        }
        finally
        {
            RequestAnimationUpdate?.Invoke(); 
            IsLoading = false;
        }
    }

    private async Task NavigateToCreateCargoAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), null }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoCreatePage)}", navigationParameter);
    }

    private async Task NavigateToEditCargoAsync(Cargo cargo)
    {
        if (cargo == null)
        {
            await _uiService.ShowSnackbarWarning("Invalid cargo selected.");
            return;
        }

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), cargo }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoCreatePage)}", navigationParameter);
    }

    private async Task DeleteCargoAsync(Cargo cargo)
    {
        if (cargo == null)
        {
            await _uiService.ShowSnackbarWarning("Invalid cargo selected.");
            return;
        }

        var deleteResult = await _cargoService.DeleteCargo(cargo.Id);
        if (deleteResult.IsSuccess)
        {
            Cargos.Remove(cargo);
            await _uiService.ShowSnackbarInfoAsync(deleteResult.Data);
        }
        else
        {
            await _uiService.ShowSnackbarWarning(deleteResult.ErrorMessage);
        }
    }

    private async Task NavigateToDetailsCargoAsync(Cargo cargo)
    {
        if (cargo == null)
        {
            await _uiService.ShowSnackbarWarning("Invalid cargo selected.");
            return;
        }

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), cargo }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoDetailsPage)}", navigationParameter);
    }

    private async Task PerformSearchAsync(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            await ResetSearchAsync();
            return;
        }

        var filteredList = Cargos
            .Where(c => c.Destination.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!filteredList.Any())
        {
            await _uiService.ShowSnackbarWarning("No cargos found matching the search criteria.");
        }
        else
        {
            Cargos = new ObservableCollection<Cargo>(filteredList);
        }
    }

    private async Task ResetSearchAsync()
    {
        await RefreshListAsync();
    }

    private async void LoadUserFirstName()
    {
        var result = await _authenticationService.GetUserFirstNameFromTokenAsync();
        if (result.IsSuccess)
        {
            UserFirstName = result.Data;
        }
        else
        {
            await _uiService.ShowSnackbarWarning(result.ErrorMessage);
        }
    }

    private async void LoadUserFunction()
    {
        var result = await _functionAccessService.GetUserFunctionFromTokenAsync();
        if (result.IsSuccess)
        {
            UserFunction = result.Data;
        }
        else
        {
            await _uiService.ShowSnackbarWarning(result.ErrorMessage);
        }
    }

    private async Task OnSearchTextChanged(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            await ResetSearchAsync();
        }
        else
        {
            await PerformSearchAsync(newText);
        }
    }
    
    public void UpdateSnowVisibility()
    {
        SnowVisibility = SnowVisibilityHelper.DetermineSnowVisibility();
    }
    private void InitializeSubscriptionSnow()
    {
        MessagingCenter.Subscribe<SettingsViewModel, bool>(this, "SnowToggleChanged", (sender, isEnabled) =>
        {
            UpdateSnowVisibility();
        });
    }
    private void InitializeSubscriptionsCargoListUpdatedInApp()
    {
        MessagingCenter.Subscribe<CargoCreatePage, bool>(this, "CargoListUpdatedInApp", async (sender, isUpdated) =>
        {
            if (isUpdated)
            {
                await RefreshListAsync();
            }
        });
    }
    private void InitializeSubscriptionsCargoListUpdatedRemotely()
    {
#if ANDROID 
        MessagingCenter.Subscribe<MyFirebaseMessagingService, bool>(this, "CargoListUpdatedRemotely", async (sender, isUpdated) =>
        {
            if (isUpdated)
            {
                await RefreshListAsync();
            }
        });
#elif IOS
        MessagingCenter.Subscribe<NotificationDelegate, bool>(this, "CargoListUpdatedRemotely", async (sender, isUpdated) =>
        {
            if (isUpdated)
            {
                await RefreshListAsync();
            }
        });
        #endif
    }
    public void Cleanup()
    {
      //  MessagingCenter.Unsubscribe<SettingsViewModel, bool>(this, "SnowToggleChanged");
        MessagingCenter.Unsubscribe<App, string>(this, "CargoListUpdatedInApp");
        MessagingCenter.Unsubscribe<App, string>(this, "CargoListUpdatedRemotely");
    }

    #endregion
}