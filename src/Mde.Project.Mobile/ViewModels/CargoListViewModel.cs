using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web.Dtos.Cargos;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class CargoListViewModel:ObservableObject{
    
    private ObservableCollection<Cargo> cargos;
    private readonly ICargoService _cargoService;
    private readonly IAuthenticationServiceMobile _authenticationService;
    private readonly IUiService _uiService;
    private Function _userFunction;
    public ObservableCollection<Cargo> Cargos
    {
        get { return cargos; }
        set
        {
            SetProperty(ref cargos, value);
        }
    }
    
    public CargoListViewModel(ICargoService cargoService, IUiService uiService, IAuthenticationServiceMobile authenticationService)
    {
        _cargoService = cargoService;
        _uiService = uiService;
        _authenticationService = authenticationService;
        LoadUserFunction();
    }
    
    public Function UserFunction
    {
        get => _userFunction;
        set => SetProperty(ref _userFunction, value);
    }

    private async void LoadUserFunction()
    {
        try
        {
            UserFunction = await _authenticationService.GetUserFunctionFromTokenAsync();
        }
        catch (Exception ex)
        {
           
        }
    }
    
    //=========================== REFRESH =====================================
    public ICommand RefreshListCommand => new Command(async () =>
    {
        
        try
        {
            var userId = await _authenticationService.GetUserIdFromTokenAsync();
            var dtoCargos = await _cargoService.GetCargosForUser(Guid.Parse(userId));
            

            // Convert DTOs to Models
            var modelCargos = dtoCargos.Select(dto => new Cargo
            {
                Id = dto.Id,
                Destination = dto.Destination,
                IsDangerous = dto.IsDangerous,
                TotalWeight = dto.TotalWeight ?? 0
            }).ToList();

            
            Cargos = new ObservableCollection<Cargo>(modelCargos);

            
            if (!modelCargos.Any()){
                _uiService.ShowSnackbarWarning("You don't have any cargos available");
            }
            if (!modelCargos.Any()){
                _uiService.ShowSnackbarWarning("You don't have any cargos available");
            }
        }
        catch (Exception ex)
        {
            _uiService.ShowSnackbarWarning("Contact your admin with explanation of the error");
        }
    });
    
    //=========================== CREATE =====================================
    public ICommand CreateCargoCommand => new Command(async () =>
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), null }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoCreatePage)}", navigationParameter);
    });
    
//=========================== EDIT =====================================
    public ICommand EditCargoCommand => new Command<Cargo>(async (cargo) =>
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), cargo }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoCreatePage)}", navigationParameter);
    });
    
    //=========================== DELETE =====================================
    public ICommand DeleteCargoCommand => new Command<Cargo>(async (cargo) =>
    {
        
        if (cargo != null)
        {
            await _cargoService.Delete(cargo.Id);
            Cargos.Remove(cargo); 
            await _uiService.ShowSnackbarDeleteAsync("CARGO DELETED SUCCESSFULLY ❌");
           
        }
        else
        {
            // Log error or handle the null case
        }
        
    });
    //=========================== SHOW =====================================
    public ICommand DetailsCargoCommand => new Command<Cargo>(async (cargo) =>
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(CargoCreateViewModel.SelectedCargo), cargo }
        };

        await Shell.Current.GoToAsync($"{nameof(CargoDetailsPage)}", navigationParameter);
    });

}