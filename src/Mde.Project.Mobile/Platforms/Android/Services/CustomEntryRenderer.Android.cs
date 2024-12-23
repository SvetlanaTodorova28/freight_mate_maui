using Android.Content;
using Mde.Project.Mobile.Platforms;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Mde.Project.Mobile.Platforms;


public class CustomEntryRenderer : EntryRenderer
{
    public CustomEntryRenderer(Context context) : base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        if (Control != null)
        {
            try
            {
               
                Control.SetTextCursorDrawable(Resource.Drawable.cursor_color); 
            }
            catch (Exception ex)
            {
               
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}