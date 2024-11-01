using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace Mde.Project.Mobile.Pages;

public partial class LoadingPageBase : ContentPage{
    private float _angle;

    public LoadingPageBase()
    {
        InitializeComponent();
        Device.StartTimer(TimeSpan.FromMilliseconds(100), () => 
        {
            _angle += 5;
            if (_angle > 360) _angle = 0;
            canvasView.InvalidateSurface(); 
            return true; 
        });
    }

    private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        
        var width = e.Info.Width;
        var height = e.Info.Height;
        var radius = Math.Min(width, height) / 8;

        var center = new SKPoint(width / 2, height / 2);
        var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            IsAntialias = true
        };

        
        var markerPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.OrangeRed
        };

        canvas.Translate(center.X, center.Y);
        canvas.RotateDegrees(_angle);

       
        canvas.DrawCircle(0, 0, radius, paint);

       
        canvas.DrawCircle(radius, 0, 20, markerPaint); 
    }


}