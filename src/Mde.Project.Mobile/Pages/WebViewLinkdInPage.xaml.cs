

namespace Mde.Project.Mobile.Pages;

[QueryProperty(nameof(Url), "url")]
public partial class WebViewLinkdInPage : ContentPage
{
    private string url;
        
    public string Url
    {
        get => url;
        set
        {
            url = Uri.UnescapeDataString(value ?? string.Empty);
            OnPropertyChanged();
            webView.Source = url; 
        }
    }

    public WebViewLinkdInPage()
    {
        InitializeComponent();
    }
}
