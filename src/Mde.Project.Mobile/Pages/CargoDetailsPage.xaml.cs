using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoDetailsPage : ContentPage{
    public CargoDetailsPage(CargoDetailsViewModel cargoDetailsViewModel){
        InitializeComponent();
        BindingContext = cargoDetailsViewModel;
    }


    private void OpenFullAddress_OnTapped(object? sender, TappedEventArgs e){
        this.ShowPopup(new PopupPage());
    }
}