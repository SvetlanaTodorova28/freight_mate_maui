
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain.Services.Web;
using Mde.Project.Mobile.ViewModels;
using SkiaSharp;

namespace Mde.Project.Mobile.Pages;

public partial class CargoCreatePage : ContentPage{

    private readonly CargoCreateViewModel _cargoCreateViewModel;
    private readonly IUiService _uiService;
    private readonly IMainThreadInvoker _mainThreadInvoker;
    private readonly IPermissionService _permissionService;
    private float _angle;
    
    public CargoCreatePage(CargoCreateViewModel cargoCreateViewModel, IUiService uiService, IMainThreadInvoker mainThreadInvoker,
        IPermissionService permissionService)
    { 
        InitializeComponent();
        BindingContext = _cargoCreateViewModel = cargoCreateViewModel;
        _uiService = uiService;
        _mainThreadInvoker = mainThreadInvoker;
        _permissionService = permissionService;
    }

    protected override void OnAppearing(){
        _cargoCreateViewModel?.OnAppearingCommand.Execute(null);
        Device.StartTimer(TimeSpan.FromMilliseconds(100), () => 
        {
            _angle += 5;
            if (_angle > 360) _angle = 0;
            canvasView.InvalidateSurface(); 
            return true; 
        });
    }
    

    private async void CreateCargo_fromPdf_OnClicked(object sender, EventArgs e)
    {
        try{
            bool hasPermission = await _permissionService
                                     .RequestIfNotGrantedAsync<Permissions.StorageWrite>() ==
                                 PermissionStatus.Granted;
            if (hasPermission){
                
                Stream pdfStream = await _uiService.PickAndOpenFileAsync("application/pdf");

                if (pdfStream == null){
                    await _uiService.ShowSnackbarWarning("No PDF file selected.");
                    return;
                }


                await Navigation.PushModalAsync(new LoadingPage());

                bool isCreated = await _cargoCreateViewModel.UploadAndProcessPdfAsync(pdfStream);
                if (isCreated){
                    MessagingCenter.Send(this, "CargoListUpdatedInApp", true);
                }
                else{
                    await Navigation.PopModalAsync();
                }

            }
            
        }
        catch (Exception ex){
            await _uiService.ShowSnackbarWarning("Failed to add cargo. Please check your data and try again");
        }
    
    }
    

    private async void ScanCargoDocument_OnClicked(object sender, EventArgs e)
    {
        try
        {
            
            var documentStream = await CaptureDocumentFromCameraAsync();
            if (documentStream == null)
            {
                await _uiService.ShowSnackbarWarning("No document scanned or failed to read the document stream.");
                return;
            }
            
            await Navigation.PushModalAsync(new LoadingPage());

            bool isCreated = await _cargoCreateViewModel.UploadAndProcessPdfAsync(documentStream,"jpeg");
            if (isCreated)
            {
                MessagingCenter.Send(this, "CargoListUpdated", true); 
            }
                                                                                 
        }
        catch (Exception ex)
        {
          
            await _uiService.ShowSnackbarWarning("Failed to add cargo. Please check your data and try again");
        }
       
    }

    public async Task<Stream> CaptureDocumentFromCameraAsync(){
        bool hasPermission = await _permissionService
                                 .RequestIfNotGrantedAsync<Permissions.Camera>() ==
                             PermissionStatus.Granted;
        if (hasPermission){

            var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions{
                Title = "Please scan the document"
            });

            if (photo != null){
                return await photo.OpenReadAsync();
            }

            return null;
        }
        return null;
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

        // Draw the dots
        canvas.DrawCircle(centerX1, centerY, baseRadius * scale1, paint);
        canvas.DrawCircle(centerX2, centerY, baseRadius * scale2, paint);
        canvas.DrawCircle(centerX3, centerY, baseRadius * scale3, paint);

        
        _angle += 5;
        if (_angle >= 360) _angle = 0;

        canvasView.InvalidateSurface();  
    }

    private async void SaveCargo_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoadingPage()); 
        bool isCreated = false;

        try
        {
            isCreated = await _cargoCreateViewModel.SaveCargoAsync();
            if (isCreated)
            {
                MessagingCenter.Send(this, "CargoUpdated", isCreated);
            }
           
        }
        catch (Exception ex)
        {
            await _uiService.ShowSnackbarWarning("Failed to add cargo. Please check your data and try again");
        }
        finally
        {
            await Navigation.PopModalAsync(); 
        }
    }
    
}