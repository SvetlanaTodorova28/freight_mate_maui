using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;

namespace Mde.Project.Mobile.Pages;

public partial class HomePage : ContentPage{
    public HomePage(HomeViewModel homeViewModel){
        InitializeComponent();
        BindingContext = homeViewModel;
    }
    private async void CreateAccount_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/createAccount");
    }
    private async void OnLoginClicked(object sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/CagosPage");
    }

  
}