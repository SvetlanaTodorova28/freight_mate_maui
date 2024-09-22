using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Mde.Project.Mobile.Pages;

public partial class OptionsPage : ContentPage{
    public OptionsPage(){
        InitializeComponent();
    }
    private async void NavigateToAbout(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/about");
    }

    private async void OpenEmailApp(object sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/about");
    }
}