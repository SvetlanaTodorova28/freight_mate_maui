namespace Mde.Project.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("cargodetails", typeof(CargoInfoPage));
        }
        private async void SiteLink_OnClicked(object? sender, EventArgs e){
            await Launcher.OpenAsync("https://bold.pro/my/todorova-svetlana/310r");
        }
        
    }
}
