using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoDetailsPage : ContentPage{
    public CargoDetailsPage(CargoDetailsViewModel cargoDetailsViewModel){
        InitializeComponent();
        BindingContext = cargoDetailsViewModel;
    }
    
    protected override void OnAppearing()
    {
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        HomeViewModel viewmodelHome = BindingContext as HomeViewModel;
        viewmodel.RefreshListCommand?.Execute(null);
        base.OnAppearing();
    }
}