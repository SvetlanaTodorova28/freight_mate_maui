
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
        CargoListViewModel viewmodel = BindingContext as CargoListViewModel;
        viewmodel.CargosLoaded += OnCargosLoaded;  
        viewmodel.RefreshListCommand?.Execute(null);
       
        Device.StartTimer(TimeSpan.FromMilliseconds(100), () => {
            _angle += 1;
            if (_angle > 360) _angle = 0;
            canvasView.InvalidateSurface();
            return true;
        });
        
    }
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
       
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

        

        int numberOfDots = 5;
        float baseRadius = 20; 
        float padding = 40; 

        var paint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColor.Parse("#4FB9FF"), 
            IsAntialias = true 
        };

        float startX = (width - (numberOfDots - 1) * padding) / 2;

        for (int i = 0; i < numberOfDots; i++)
        {
            float angleOffset = _angle + i * 150; 
            float scale = 0.5f + 0.5f * MathF.Sin(MathF.PI * angleOffset / 180); 
            float scaledRadius = baseRadius * scale;
            float x = startX + i * padding;
            float y = height / 2;

            canvas.DrawCircle(x, y, scaledRadius, paint);
        }

       
        _angle += 1;
        if (_angle >= 360) _angle = 0;

        canvasView.InvalidateSurface();
    }







    

}