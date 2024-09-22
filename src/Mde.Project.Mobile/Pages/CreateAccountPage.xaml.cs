using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Pages;

public partial class CreateAccountPage : ContentPage{
    public CreateAccountPage(){
        InitializeComponent();
    }
    private async void CreateAccount_Clicked(object sender, EventArgs e){
        await Shell.Current.GoToAsync("//pages/home");
    }
}