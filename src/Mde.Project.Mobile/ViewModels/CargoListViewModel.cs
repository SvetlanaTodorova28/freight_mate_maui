using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class CargoListViewModel:ObservableObject{
    
    private ObservableCollection<Cargo> cargos;
    private readonly IWebCargoService cargoService;
    private readonly IUiService uiService;
    
    public ObservableCollection<Cargo> Cargos
    {
        get { return cargos; }
        set
        {
            SetProperty(ref cargos, value);
        }
    }
    
    public CargoListViewModel(IWebCargoService cargoService, IUiService uiService)
    {
        this.cargoService = cargoService;
        this.uiService = uiService;
    }
    
    
    
    //=========================== REFRESH =====================================
    public ICommand RefreshListCommand => new Command(async () =>
    {
        var cargos = await cargoService.GetAll();
        Cargos = new ObservableCollection<Cargo>(cargos);
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
            await cargoService.Delete(cargo);
            Cargos.Remove(cargo); 
            await uiService.ShowSnackbarDeleteAsync("CARGO DELETED SUCCESSFULLY ❌");
           
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