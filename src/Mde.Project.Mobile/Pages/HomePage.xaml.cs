using Mde.Project.Mobile.ViewModels;


namespace Mde.Project.Mobile.Pages;

public partial class HomePage : ContentPage{
    
    
    public HomePage(HomeViewModel homeViewModel){
        InitializeComponent();
        BindingContext = homeViewModel;
    }
   

  
}