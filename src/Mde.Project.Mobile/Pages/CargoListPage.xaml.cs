using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoListPage : ContentPage{
    public CargoListPage(CargoListViewModel cargoListViewModel){
        InitializeComponent();
        BindingContext = cargoListViewModel;
    }
    private async void Add_Cargo_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/createCargo");
    }

    private async  void Button1_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/infoCargo");
    }
}