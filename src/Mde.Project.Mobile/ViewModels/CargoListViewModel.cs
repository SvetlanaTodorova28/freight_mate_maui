using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class CargoListViewModel:ObservableObject{
    
   
    private readonly ICargoService _cargoService;
    private readonly IAuthenticationServiceMobile _authenticationService;
    private readonly IUiService _uiService;
   
   
    public CargoListViewModel(ICargoService cargoService, IUiService uiService, IAuthenticationServiceMobile authenticationService)
    {
        _cargoService = cargoService;
        _uiService = uiService;
        _authenticationService = authenticationService;
        RefreshListCommand = new AsyncRelayCommand(RefreshListAsync);
        CreateCargoCommand = new AsyncRelayCommand(NavigateToCreateCargoAsync);
        EditCargoCommand = new AsyncRelayCommand<Cargo>(NavigateToEditCargoAsync);
        DeleteCargoCommand = new AsyncRelayCommand<Cargo>(DeleteCargoAsync);
        DetailsCargoCommand = new AsyncRelayCommand<Cargo>(NavigateToDetailsCargoAsync);
        PerformSearchCommand = new AsyncRelayCommand<string>(PerformSearchAsync);
        TextChangedCommand = new AsyncRelayCommand<string>(OnSearchTextChanged);
        LoadUserFunction();
        LoadUserFirstName();
    }

    #region Bindings
    
    private ObservableCollection<Cargo> cargos = new ObservableCollection<Cargo>();
    public ObservableCollection<Cargo> Cargos
    {
        get { return cargos; }
        set
        {
            SetProperty(ref cargos, value);
        }
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

    # region methods
    
    public event EventHandler CargosLoaded;

  
   
    private async Task RefreshListAsync()
    {
        IsLoading = true;
       
        try
        {
            var userId = await _authenticationService.GetUserIdFromTokenAsync();
            if (string.IsNullOrEmpty(userId))
            {
                _uiService.ShowSnackbarWarning("User ID is not available, please login again.");
                return;
            }

            var dtoCargos = await _cargoService.GetCargosForUser(Guid.Parse(userId));

            var modelCargos = dtoCargos.Select(dto => new Cargo
            {
                Id = dto.Id,
                Destination = dto.Destination,
                IsDangerous = dto.IsDangerous,
                TotalWeight = dto.TotalWeight ?? 0,
                Userid = Guid.Parse(userId)
            }).ToList();

            Cargos = new ObservableCollection<Cargo>(modelCargos);
            CargosLoaded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            
            Cargos = new ObservableCollection<Cargo>(); 
        }
        finally
        {
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
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), cargo }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoCreatePage)}", navigationParameter);
    }

    private async Task DeleteCargoAsync(Cargo cargo)
    {
        if (cargo != null)
        {
            await _cargoService.DeleteCargo(cargo.Id);
            Cargos.Remove(cargo); 
            await _uiService.ShowSnackbarDeleteAsync("CARGO DELETED SUCCESSFULLY ❌");
           
        }
        else
        {
            await _uiService.ShowSnackbarWarning("ChECK IF CARGO EXISTS IN THE LIST FIRST! ��");
        }
    }

    private async Task NavigateToDetailsCargoAsync(Cargo cargo)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), cargo }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoDetailsPage)}", navigationParameter);
    }

    private async Task PerformSearchAsync(string searchTerm)
    {
        var filteredList = await Task.Run(() => Cargos
            .Where(c => c.Destination.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList());

        if (!filteredList.Any())
        {
            _uiService.ShowSnackbarWarning("No cargos found matching the search criteria.");
        }
        else
        {
            Cargos = new ObservableCollection<Cargo>(filteredList);
        }
    }
    private async void LoadUserFirstName()
    {
        try
        {
            UserFirstName = await _authenticationService.GetUserFirstNameFromTokenAsync();
        }
        catch (Exception ex)
        {
            _uiService.ShowSnackbarWarning("Could not load user first name.");
        }
    }
    
    private async void LoadUserFunction()
    {
        try
        {
            UserFunction = await _authenticationService.GetUserFunctionFromTokenAsync();
        }
        catch (Exception ex)
        {
            _uiService.ShowSnackbarWarning("Could not load the functions.");
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
    private async Task ResetSearchAsync()
    {
       
        await RefreshListAsync();
    }
    #endregion
    
    
   

}