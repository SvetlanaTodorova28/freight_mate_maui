using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Mde.Project.Mobile.Pages;

public partial class CargosPage : ContentPage{
    public CargosPage(){
        InitializeComponent();
    }
    private async void Add_Cargo_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/createCargo");
    }

    private async  void Button1_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/infoCargo");
    }
}