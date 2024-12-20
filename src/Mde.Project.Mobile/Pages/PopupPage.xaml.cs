
using CommunityToolkit.Maui.Views;

namespace Mde.Project.Mobile.Pages;

public partial class PopupPage : Popup{
    
    public PopupPage(){
        InitializeComponent();
    }
    
    private void Label_Tapped(object sender, EventArgs e)
    {
        Close();
    }
}