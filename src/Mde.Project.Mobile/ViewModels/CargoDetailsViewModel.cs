using System.Windows.Input;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.ViewModels;

[QueryProperty(nameof(SelectedCargo), nameof(SelectedCargo))]
public class CargoDetailsViewModel{
    private readonly ICargoService cargoService;
    private readonly IUiService uiService;
    
    
    private Cargo selectedCargo;
    public Cargo SelectedCargo
    {
        get { return selectedCargo; }
        set
        {
            selectedCargo = value;
            
        }
        
    }
    public ICommand DeleteCommand => new Command(async () =>
    {
        //todo: validation
        Cargo cargo;
        cargo = SelectedCargo;
        
            await cargoService.Delete(cargo);
            await uiService.ShowSnackbarDeleteAsync("CARGO DELETED SUCCESSFULLY 📦");
       

        await Shell.Current.GoToAsync("//CargoListPage");
           

    });
}