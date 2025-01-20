
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.ViewModels;
using SkiaSharp.Views.Maui;
using SkiaSharp;

namespace Mde.Project.Mobile.Pages;

public partial class CargoListPage : ContentPage{
    private readonly IMainThreadInvoker _mainThreadInvoker;
    private readonly CargoService _cargoService;
    private readonly CargoListViewModel _cargoListViewModel;
    private float _angle;
    private Timer _animationTimer;
   
    
    public CargoListPage(CargoListViewModel cargoListViewModel, IMainThreadInvoker mainThreadInvoker){ 
        InitializeComponent();
        BindingContext = _cargoListViewModel =  cargoListViewModel;
        _mainThreadInvoker = mainThreadInvoker;
    }
   
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
       
        _cargoListViewModel.RequestAnimationUpdate += () =>
        {
            _mainThreadInvoker.InvokeOnMainThread(() => canvasView.InvalidateSurface());
        };
        _cargoListViewModel.CargosLoaded += OnCargosLoaded;  
        _cargoListViewModel.RefreshListCommand?.Execute(null);
        
        Device.StartTimer(TimeSpan.FromMilliseconds(100), () => 
        {
            _angle += 5;
            if (_angle > 360) _angle = 0;
            canvasView.InvalidateSurface(); 
            return true; 
        });
        
    }
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
       _cargoListViewModel.Cleanup();
       _cargoListViewModel.RequestAnimationUpdate -= () =>
       {
           _mainThreadInvoker.InvokeOnMainThread(() => canvasView.InvalidateSurface());
       };
    }
   
    private void OnCargosLoaded(object sender, EventArgs e)
    {
        _mainThreadInvoker.InvokeOnMainThread(() => {
            canvasView.InvalidateSurface(); 
        });
    }
   
    private void LstCargos_OnItemTapped(object? sender, ItemTappedEventArgs e){
        Cargo cargo = e.Item as Cargo;
        _cargoListViewModel.DetailsCargoCommand?.Execute(cargo);
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var width = e.Info.Width;
        var height = e.Info.Height;
    
         
       canvas.Clear(SKColors.Transparent); 

        var baseRadius = 20f;  
        var distance = 60f;  
        var centerY = height / 2;
        var centerX = width / 2;

        var paint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color =SKColor.Parse("#4FB9FF"), 
            IsAntialias = true
        };

       
        var centerX1 = centerX - distance;  
        var centerX2 = centerX;             
        var centerX3 = centerX + distance;  

       
        var scale1 = 0.5f + 0.5f * MathF.Sin(MathF.PI * _angle / 180);
        var scale2 = 0.5f + 0.5f * MathF.Sin(MathF.PI * (_angle + 120) / 180);  
        var scale3 = 0.5f + 0.5f * MathF.Sin(MathF.PI * (_angle + 240) / 180);  

      
        canvas.DrawCircle(centerX1, centerY, baseRadius * scale1, paint);
        canvas.DrawCircle(centerX2, centerY, baseRadius * scale2, paint);
        canvas.DrawCircle(centerX3, centerY, baseRadius * scale3, paint);

        
        _angle += 5;
        if (_angle >= 360) _angle = 0;

        canvasView.InvalidateSurface();  
    }





    

}