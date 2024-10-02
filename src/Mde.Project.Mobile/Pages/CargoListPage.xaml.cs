using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoListPage : ContentPage{
    
    public CargoListPage(CargoListViewModel cargoListViewModel){
        InitializeComponent();
        BindingContext = cargoListViewModel;
    }
   
    
    protected override void OnAppearing()
    {
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        HomeViewModel viewmodelHome = BindingContext as HomeViewModel;
        viewmodel.RefreshListCommand?.Execute(null);
        base.OnAppearing();
    }
   

    private void LstCargos_OnItemTapped(object? sender, ItemTappedEventArgs e){
        Cargo cargo = e.Item as Cargo;
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        viewmodel.DetailsCargoCommand?.Execute(cargo);
    }
}