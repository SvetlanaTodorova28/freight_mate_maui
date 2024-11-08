using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    
    public void OnImageButtonClicked(object sender, EventArgs e)
    {
       
        CargoDetailsViewModel viewmodel = BindingContext as CargoDetailsViewModel;
        viewmodel.NavigateCommand.Execute(null);
    }

    
   
}