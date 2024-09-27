using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;


namespace Mde.Project.Mobile.Pages;

public partial class CargoInfoPage : ContentPage{
    public CargoInfoPage(){
        InitializeComponent();
    }
    private async void btnDelete_Clicked(object? sender, EventArgs e){

        var snackbarOptions = new SnackbarOptions{
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            Font = Font.SystemFontOfSize(16),
            CharacterSpacing = 1,
            ActionButtonTextColor = Colors.White
        };

        string text = "Are you sure you want to delete";
        TimeSpan duration = TimeSpan.FromSeconds(5);
        Action buttonAction = async () => await DisplayAlert("You chose to delete the cargo", "Are you sure you want to delete this cargo", "Yes");
         
        string submit = "Delete the cargo";

        var snackbar = Snackbar.Make(text, buttonAction, submit, duration, snackbarOptions);
        await snackbar.Show();
    }
    private async void btnUpdate_Clicked(object? sender, EventArgs e){

        var snackbarOptions = new SnackbarOptions{
            BackgroundColor = Colors.DarkOrange,
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
            Font = Font.SystemFontOfSize(16),
            CharacterSpacing = 1,
            ActionButtonTextColor = Colors.White
        };

        string text = "Are you sure you want to delete";
        TimeSpan duration = TimeSpan.FromSeconds(15);
        Action buttonAction = async () => await DisplayAlert("You chose to update the cargo", "Are you sure you want to update this cargo", "Yes");
         
        string submit = "Update the cargo";

        var snackbar = Snackbar.Make(text, buttonAction, submit, duration, snackbarOptions);
        await snackbar.Show();
    }
}