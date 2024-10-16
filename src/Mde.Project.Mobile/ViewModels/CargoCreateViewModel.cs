using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Models;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile.ViewModels;


[QueryProperty(nameof(SelectedCargo), nameof(SelectedCargo))]
public class CargoCreateViewModel:ObservableObject{
    
    
    private readonly IWebCargoService cargoService;
    private readonly IUiService uiService;
    

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
                    Destination = selectedCargo.Destination;
                    TotalWeight = selectedCargo.TotalWeight;
                    IsDangerous = selectedCargo.IsDangerous;
                }
                else
                {
                    PageTitle = "Add cargo";
                    Destination = default;
                    TotalWeight = default;
                    IsDangerous = default;

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
        
        
        public CargoCreateViewModel(IWebCargoService cargoService, IUiService uiService)
        {
            this.cargoService = cargoService;
            this.uiService = uiService;
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

            cargo.TotalWeight = TotalWeight;
            cargo.Destination = Destination;
            cargo.IsDangerous = IsDangerous;
            cargo.Destination = Destination;

            if (cargo.Id.Equals(Guid.Empty))
            {
                await cargoService.Add(cargo);
                await uiService.ShowSnackbarSuccessAsync("CARGO CREATED SUCCESSFULLY 📦");
            }
            else
            {
                await cargoService.Update(cargo);
               
            }

            await Shell.Current.GoToAsync("//CargoListPage");
           

        });

    }
