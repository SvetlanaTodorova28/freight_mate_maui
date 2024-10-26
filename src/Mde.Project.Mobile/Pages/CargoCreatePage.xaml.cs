using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoCreatePage : ContentPage{

    private readonly CargoCreateViewModel _cargoCreateViewModel;
    
    public CargoCreatePage(CargoCreateViewModel cargoCreateViewModel){ 
        InitializeComponent();
        BindingContext = _cargoCreateViewModel = cargoCreateViewModel;
    }

    protected override void OnAppearing(){
        _cargoCreateViewModel?.OnAppearingCommand.Execute(null);
    }


}