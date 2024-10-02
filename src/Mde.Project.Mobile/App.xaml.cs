using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Pages;

namespace Mde.Project.Mobile;

    public partial class App : Application
    {
        private readonly ICargoService cargoService;
        private readonly IUiService uiService;

        public App(ICargoService cargoService, IUiService uiService){
            this.cargoService = cargoService;
            this.uiService = uiService;
            {

                InitializeComponent();
                MainPage = new NavigationPage(new MainPage(uiService, cargoService));
            }
        }
    }
