
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;

namespace Mde.Project.Mobile.Pages;

public partial class CargoCreatePage : ContentPage{

    private readonly CargoCreateViewModel _cargoCreateViewModel;
    private readonly IUiService _uiService;
    
    public CargoCreatePage(CargoCreateViewModel cargoCreateViewModel, IUiService uiService){ 
        InitializeComponent();
        BindingContext = _cargoCreateViewModel = cargoCreateViewModel;
        _uiService = uiService;
    }

    protected override void OnAppearing(){
        _cargoCreateViewModel?.OnAppearingCommand.Execute(null);
    }
    

    private async void CreateCargo_fromPdf_OnClicked(object sender, EventArgs e)
    {
        try
        {
            
            Stream pdfStream = await _uiService.PickAndOpenFileAsync("application/pdf");

            if (pdfStream == null)
            {
                await _uiService.ShowSnackbarWarning("No PDF file selected.");
                return;
            }

           
            await Navigation.PushModalAsync(new LoadingPageCreateCargoFromPdf(), true);

          
            bool isCreated = await _cargoCreateViewModel.UploadAndProcessPdfAsync(pdfStream);

            if (isCreated)
            {
                await _uiService.ShowSnackbarSuccessAsync("Your cargo is successfully created.");
                await Shell.Current.GoToAsync("//CargoListPage");
            }
            else
            {
                await _uiService.ShowSnackbarWarning("Failed to create cargo from PDF.");
            }
        }
        catch (Exception ex)
        {
            await _uiService.ShowSnackbarWarning($"An error occurred: {ex.Message}");
        }
        finally
        {
           
            await Navigation.PopModalAsync(true);
        }
    }


    private async void ScanCargoDocument_OnClicked(object sender, EventArgs e)
    {
        try
        {
            // Aangepaste methode om de camera te openen en een foto vast te leggen
            var documentStream = await CaptureDocumentFromCameraAsync();

            if (documentStream == null)
            {
                await _uiService.ShowSnackbarWarning("No document scanned or failed to read the document stream.");
                return;
            }

           
            /*await Navigation.PushModalAsync(new LoadingPageCreateCargoFromPdf(), true);*/

            // Verwerk het document via de ViewModel
            bool isCreated = await _cargoCreateViewModel.UploadAndProcessPdfAsync(documentStream);

            // Verwijder het laadscherm
            /*await Navigation.PopModalAsync(true);*/

            // Toon het resultaat van het verwerken van het document
            if (isCreated)
            {
                await _uiService.ShowSnackbarSuccessAsync("Your cargo is successfully created.");
                await Shell.Current.GoToAsync("//CargoListPage");
            }
            else
            {
                await _uiService.ShowSnackbarWarning("Failed to create cargo from the scanned document.");
            }
        }
        catch (Exception ex)
        {
            await Navigation.PopModalAsync(true);
            await _uiService.ShowSnackbarWarning($"An error occurred: {ex.Message}");
        }
    }

// Aparte methode om de camera te gebruiken en een foto vast te leggen
    public async Task<Stream> CaptureDocumentFromCameraAsync()
    {
        var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
        {
            Title = "Please scan the document"
        });

        if (photo != null)
        {
            return await photo.OpenReadAsync();
        }
        return null;
    }


}