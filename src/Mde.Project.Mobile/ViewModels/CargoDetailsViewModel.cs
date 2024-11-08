using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;



namespace Mde.Project.Mobile.ViewModels;

[QueryProperty(nameof(SelectedCargo), nameof(SelectedCargo))]
public class CargoDetailsViewModel:ObservableObject{
    
    public CargoDetailsViewModel(){
        NavigateCommand = new AsyncRelayCommand<string>(OpenNavigationApp);
    }

    private Cargo selectedCargo;
    public Cargo SelectedCargo
    {
        get { return selectedCargo; }
        set
        {
            selectedCargo = value;

                
            if (selectedCargo != null)
            {
               
                Destination = selectedCargo.Destination;
                TotalWeight = selectedCargo.TotalWeight?? 0;
                IsDangerous = selectedCargo.IsDangerous;
            }
            else{
                Console.Error.WriteLine("Cannot find Cargo with this id");

            }
        }
    }
    
    private double totalWeight;
    public double TotalWeight
    {
        get { return totalWeight; }
        set{
            SetProperty(ref totalWeight, value);
        }
    }
    
    private bool isDangerous;
    public bool IsDangerous
    {
        get { return isDangerous; }
        set{
            SetProperty(ref isDangerous, value);

        }
    }
    
    private string destination;
    public string Destination
    {
        get { return destination; }
        set{
            SetProperty(ref destination, value);
        }
    }
    
   
  
    public ICommand NavigateCommand { get; }
    
       

    
    private async Task OpenNavigationApp(string destination)
    {
        try
        {
            var locations = await Geocoding.GetLocationsAsync(destination);
            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                var confirm = await App.Current.MainPage.DisplayAlert("Confirm Location",
                    $"Do you want to navigate to this address: {destination} at latitude: {location.Latitude}, longitude: {location.Longitude}?",
                    "Yes", "No");
                if (confirm)
                {
                    var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                    await Map.OpenAsync(location, options);
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Location Not Found",
                    "The specified location could not be resolved. Please check the address or enter it manually.", "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error",
                $"An error occurred while trying to navigate: {ex.Message}", "OK");
        }
    }

    /*private async Task OpenNavigationApp(string destination)
    {
        try
        {
            var locations = await Geocoding.GetLocationsAsync(destination);
            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                await Map.OpenAsync(location, options);
            }
            else
            {
                // Handle the case where the location isn't found
                await App.Current.MainPage.DisplayAlert("Error", "Location not found.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that may have occurred
            await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }*/



   
  
}