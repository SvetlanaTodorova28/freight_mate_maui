using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Pages;

public partial class HomePage : ContentPage{
    public HomePage(){
        InitializeComponent();
    }
    private  async void BtnTranslate_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//translate");
    }

    private async  void BtnNavigation_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//navigation");
    }

    private async void BtnCargosInfo_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//cargos");
    }
}