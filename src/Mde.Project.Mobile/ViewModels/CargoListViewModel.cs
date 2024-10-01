using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;

public class CargoListViewModel:ObservableObject{
    
    private ObservableCollection<Cargo> cargos;
    private readonly ICargoService cargoService;
    
    public ObservableCollection<Cargo> Cargos
    {
        get { return cargos; }
        set
        {
            SetProperty(ref cargos, value);
        }
    }
    
    public CargoListViewModel(ICargoService cargoService)
    {
        this.cargoService = cargoService;
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
}