using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;


namespace Mde.Project.Mobile.Pages;

public partial class CreateCargoPage : ContentPage{
    public CreateCargoPage(){
        InitializeComponent();
    }

    private async void btnCreate_Clicked(object? sender, EventArgs e){

        var snackbarOptions = new SnackbarOptions{
            BackgroundColor = Colors.DarkSeaGreen,
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            Font = Font.SystemFontOfSize(18),
            CharacterSpacing = 1
            
        };

        string text = "CARGO CREATED SUCCESSFULLY 📦";
        TimeSpan duration = TimeSpan.FromSeconds(5);
        

        var snackbar = Snackbar.Make(text, null, "", duration, snackbarOptions);
        await snackbar.Show();
    }
}
