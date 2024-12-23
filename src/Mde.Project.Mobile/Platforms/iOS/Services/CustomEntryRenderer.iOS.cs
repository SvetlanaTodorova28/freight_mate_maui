

using Mde.Project.Mobile.Platforms;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using UIKit;
[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Mde.Project.Mobile.Platforms;

public class CustomEntryRenderer:EntryRenderer{
    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        if (Control != null)
        {
            Control.BackgroundColor = UIColor.FromRGB(255, 69, 0);
        }
    }
}