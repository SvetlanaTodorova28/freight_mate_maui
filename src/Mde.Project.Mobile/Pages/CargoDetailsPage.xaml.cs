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
    
   
}