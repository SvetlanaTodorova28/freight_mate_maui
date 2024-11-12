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
        var radius = Math.Min(width, height) / 9.5f; 

        var center = new SKPoint(width / 2, height / 2);
        canvas.Translate(center.X, center.Y);
        var canvasColor = SKColor.Parse("#3B3131");
        canvas.Clear(canvasColor);

        var segmentColor = SKColor.Parse("#4FB9FF"); 
        var backgroundColor = SKColor.Parse("#D0D0D0"); 

        int totalSegments = 24; 
        float segmentDegrees = 360f / totalSegments; 

        var backgroundPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 40,
            Color = backgroundColor,
            IsAntialias = true
        };

        var progressPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 40,
            Color = segmentColor,
            IsAntialias = true
        };

       
        for (int i = 0; i < totalSegments; i++)
        {
            canvas.DrawArc(new SKRect(-radius, -radius, radius, radius), i * segmentDegrees, segmentDegrees - 4, false, backgroundPaint); 
        }

       
        int activeSegments = (int)(_angle / segmentDegrees);
        
        for (int i = 0; i < activeSegments; i++)
        {
            canvas.DrawArc(new SKRect(-radius, -radius, radius, radius), i * segmentDegrees, segmentDegrees - 4, false, progressPaint);
        }
    }



}