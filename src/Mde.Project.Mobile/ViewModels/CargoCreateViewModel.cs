using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;

namespace Mde.Project.Mobile.ViewModels;


[QueryProperty(nameof(selectedCargo), nameof(selectedCargo))]
public class CargoCreateViewModel:ObservableObject{
       private readonly ICargoService cargoService;

        private string pageTitle;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                SetProperty(ref pageTitle, value);
            }
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
                    PageTitle = "Edit cargo";
                    TotalWeight = selectedCargo.TotalWeight;
                    IsDangerous = selectedCargo.IsDangerous;
                }
                else
                {
                    PageTitle = "Add student";
                  
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
        
    

      

        public CargoCreateViewModel(ICargoService cargoService)
        {
            this.cargoService = cargoService;
        }
        
        public ICommand SaveCommand => new Command(async () =>
        {
            //todo: validation
            Cargo cargo;

            if (selectedCargo == null)
            {
                cargo = new Cargo();
            }
            else
            {
                cargo = SelectedCargo;
            }

            cargo.TotalWeight = selectedCargo.TotalWeight;
            cargo.Destination = selectedCargo.Destination;
            cargo.IsDangerous = selectedCargo.IsDangerous;
            cargo.Destination = selectedCargo.Destination;

            if (cargo.Id.Equals(Guid.Empty))
            {
                await cargoService.Add(cargo);
            }
            else
            {
                await cargoService.Update(cargo);
            }

            await Shell.Current.GoToAsync("//CargoListPage");

        });

    }
