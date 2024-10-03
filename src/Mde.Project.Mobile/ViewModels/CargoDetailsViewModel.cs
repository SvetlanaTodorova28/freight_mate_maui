using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.ViewModels;

[QueryProperty(nameof(SelectedCargo), nameof(SelectedCargo))]
public class CargoDetailsViewModel:ObservableObject{
    private readonly ICargoService cargoService;
    private readonly IUiService uiService;
    
    
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

   
  
}