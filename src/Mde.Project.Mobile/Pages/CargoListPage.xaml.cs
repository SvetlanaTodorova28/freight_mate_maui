using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoListPage : ContentPage{
    private readonly IWebCargoService _webCargoService;
    
    public CargoListPage(CargoListViewModel cargoListViewModel){
        InitializeComponent();
        BindingContext = cargoListViewModel;
    }
   
    
    protected override void OnAppearing()
    {
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        LoginViewModel viewmodelLogin = BindingContext as LoginViewModel;
        viewmodel.RefreshListCommand?.Execute(null);
        base.OnAppearing();
    }
   

    private void LstCargos_OnItemTapped(object? sender, ItemTappedEventArgs e){
        Cargo cargo = e.Item as Cargo;
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        viewmodel.DetailsCargoCommand?.Execute(cargo);
    }
}