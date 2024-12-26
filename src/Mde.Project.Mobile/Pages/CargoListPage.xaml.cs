
using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using SkiaSharp;

namespace Mde.Project.Mobile.Pages;

public partial class CargoListPage : ContentPage{
    private readonly ICargoService _cargoService;
    private float _angle;
    private Timer _animationTimer;
   
    
    public CargoListPage(CargoListViewModel cargoListViewModel){
        InitializeComponent();
        BindingContext = cargoListViewModel;
    }
   
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        MessagingCenter.Subscribe<CargoCreatePage, bool>(this, "CargoUpdated", async (sender, arg) =>
        {
            var viewModel = BindingContext as CargoListViewModel;
            viewModel?.RefreshListCommand.Execute(null);
            await Task.Delay(1000); 
            Device.BeginInvokeOnMainThread(() => canvasView.InvalidateSurface()); 
        });
       
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        viewmodel.CargosLoaded += OnCargosLoaded;  
        viewmodel.RefreshListCommand?.Execute(null);
        
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
        MessagingCenter.Unsubscribe<CargoCreatePage>(this, "CargoUpdated");
        if (BindingContext is CargoListViewModel viewModel) {
            viewModel.CargosLoaded -= OnCargosLoaded;
        }
    }
   
    private void OnCargosLoaded(object sender, EventArgs e)
    {
        Device.BeginInvokeOnMainThread(() => {
            canvasView.InvalidateSurface(); 
        });
    }
   
    private void LstCargos_OnItemTapped(object? sender, ItemTappedEventArgs e){
        Cargo cargo = e.Item as Cargo;
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        viewmodel.DetailsCargoCommand?.Execute(cargo);
    }

    private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
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