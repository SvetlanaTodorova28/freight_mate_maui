
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;

using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoListPage : ContentPage{
    private readonly ICargoService _cargoService;
    
    public CargoListPage(CargoListViewModel cargoListViewModel){
        InitializeComponent();
        BindingContext = cargoListViewModel;
    }
   
    
    protected override void OnAppearing()
    {
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        /*LoginViewModel viewmodelLogin = BindingContext as LoginViewModel;*/
        viewmodel.RefreshListCommand?.Execute(null);
        base.OnAppearing();
    }
   

    private void LstCargos_OnItemTapped(object? sender, ItemTappedEventArgs e){
        Cargo cargo = e.Item as Cargo;
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        viewmodel.DetailsCargoCommand?.Execute(cargo);
    }

    /*private void SearchBar_Focused(object sender, FocusEventArgs e)
    {
        SearchBar.Unfocus();
    }*/

}