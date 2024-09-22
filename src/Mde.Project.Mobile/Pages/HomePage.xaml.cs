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
    private async void CreateAccount_OnClicked(object? sender, EventArgs e){
        await Shell.Current.GoToAsync("//createAccount");
    }

    private void Login_OnClicked(object? sender, EventArgs e){
        throw new NotImplementedException();
    }
}