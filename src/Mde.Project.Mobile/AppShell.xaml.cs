using System;
using System.Windows.Input;
using Mde.Project.Mobile.Core.Service.Interfaces;
using Mde.Project.Mobile.Pages;
using Mde.Project.Mobile.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace Mde.Project.Mobile
{
    public partial class AppShell : Shell
    {
        
        private readonly IAuthenticationServiceMobile _authenticationService;
        private readonly IUiService uiService;
        private readonly AppUserRegisterViewModel _userRegisterViewModel;
        public AppShell( IAuthenticationServiceMobile authenticationService, IUiService uiService,
        AppUserRegisterViewModel userRegisterViewModel)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _userRegisterViewModel = userRegisterViewModel;
            this.uiService = uiService;
            BindingContext = new BaseViewModel(uiService, _authenticationService, _userRegisterViewModel);
          
            
        }
        private async void SiteLink_OnClicked(object? sender, EventArgs e){
            await Launcher.OpenAsync("https://bold.pro/my/todorova-svetlana/310r");
        }
        
       
        
    }
}
