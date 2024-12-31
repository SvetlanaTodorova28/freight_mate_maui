using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Converters;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class SettingsPage : ContentPage{
    private readonly SettingsViewModel _settingsViewModel;

    public SettingsPage(SettingsViewModel settingsViewModel){
        _settingsViewModel = settingsViewModel;
        InitializeComponent();
        BindingContext = _settingsViewModel;
    }

}