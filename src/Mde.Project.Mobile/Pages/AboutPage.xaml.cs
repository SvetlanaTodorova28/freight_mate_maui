using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Pages;

public partial class AboutPage : ContentPage{
    public AboutPage(){
        InitializeComponent();
    }
    private async void LabelTapped(object sender, EventArgs e)
    {
        var label = sender as Label;
        if (label != null)
        {
            var url = label.GestureRecognizers.OfType<TapGestureRecognizer>().FirstOrDefault()?.CommandParameter as string;
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    await Launcher.OpenAsync(new Uri(url));
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("Cannot open URL: " + ex.Message);
                }
            }
        }
    }
}