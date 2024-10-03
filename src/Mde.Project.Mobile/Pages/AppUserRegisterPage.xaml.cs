using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Core.Service.Interfaces;

namespace Mde.Project.Mobile.Pages;

public partial class AppUserRegisterPage : ContentPage{
    private readonly IUiService _uiService;
    private readonly 
        ICargoService _cargoService;
    public AppUserRegisterPage(IUiService uiService, ICargoService cargoService){
        InitializeComponent();
        _uiService = uiService;
        _cargoService = cargoService;
    }

    private void BackToLogin_OnTapped(object? sender, TappedEventArgs e){
        Navigation.PushAsync( new MainPage(_uiService, _cargoService));
    }
}