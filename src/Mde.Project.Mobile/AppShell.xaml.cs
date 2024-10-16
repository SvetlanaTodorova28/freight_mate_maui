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
        public AppShell( IAuthenticationServiceMobile authenticationService, IUiService uiService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            this.uiService = uiService;
            BindingContext = new BaseViewModel(uiService, _authenticationService);
          
            
        }
        private async void SiteLink_OnClicked(object? sender, EventArgs e){
            await Launcher.OpenAsync("https://bold.pro/my/todorova-svetlana/310r");
        }
        
       
        
    }
}
