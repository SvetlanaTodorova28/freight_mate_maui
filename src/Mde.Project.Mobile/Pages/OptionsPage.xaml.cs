using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Pages;

public partial class OptionsPage : ContentPage{
    public OptionsPage(){
        InitializeComponent();
    }

    private async void NavigateToAbout_OnTapped(object? sender, TappedEventArgs e){
        await Shell.Current.GoToAsync("//options/about");
    }

    private void OpenEmailApp_OnTapped(object? sender, TappedEventArgs e){
        throw new NotImplementedException();
    }
}