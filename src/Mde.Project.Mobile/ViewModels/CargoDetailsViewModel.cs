using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;


namespace Mde.Project.Mobile.ViewModels;

[QueryProperty(nameof(SelectedCargo), nameof(SelectedCargo))]
public class CargoDetailsViewModel:ObservableObject{
    
    private readonly IGeocodingService _geocodingService;
    public CargoDetailsViewModel( IGeocodingService geocodingService){
        _geocodingService = geocodingService;
        NavigateCommand = new AsyncRelayCommand<string>(OpenNavigationApp);
    }

    public DateTime CurrentDate { get; } = DateTime.Now;
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
                TotalWeight = selectedCargo.TotalWeight;
                IsDangerous = selectedCargo.IsDangerous;
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
            var locations = await _geocodingService.GetLocationsAsync(destination);
            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                bool isValidPlacemark = await ValidatePlacemark(location, destination);
                if (!isValidPlacemark)
                {
                    await App.Current.MainPage.DisplayAlert("Location Not Valid",
                        "The resolved location does not match the entered address. Please verify and try again.", "OK");
                    return;
                }

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

  
   
    private async Task<bool> ValidatePlacemark(Location location, string destination)
    {
        var placemarks = await _geocodingService.GetPlacemarksAsync(location);
        var placemark = placemarks.FirstOrDefault();
        if (placemark == null)
        {
            return false;
        }

        var inputWords = CleanString(destination).Split(new char[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

       
        var thoroughfareWords = new List<string>();

        
        if (!string.IsNullOrWhiteSpace(placemark.CountryName))
            thoroughfareWords.Add(CleanString(placemark.CountryName));
        if (!string.IsNullOrWhiteSpace(placemark.Locality))
            thoroughfareWords.Add(CleanString(placemark.Locality));
        if (!string.IsNullOrWhiteSpace(placemark.FeatureName))
            thoroughfareWords.AddRange(CleanString(placemark.FeatureName).Split(' ', StringSplitOptions.RemoveEmptyEntries));
        if (!string.IsNullOrWhiteSpace(placemark.Thoroughfare))
            thoroughfareWords.AddRange(CleanString(placemark.Thoroughfare).Split(' ', StringSplitOptions.RemoveEmptyEntries));

       
        int minimumMatchesRequired = 3;
        int matchCount = inputWords
            .Count(inputWord => thoroughfareWords.Any(thoroughfareWord => thoroughfareWord.Contains(inputWord)));

       
        return matchCount >= minimumMatchesRequired;
    }


    private string CleanString(string input)
    {
        return input.Replace("\"", "").Trim().ToLowerInvariant();
    }

    
}