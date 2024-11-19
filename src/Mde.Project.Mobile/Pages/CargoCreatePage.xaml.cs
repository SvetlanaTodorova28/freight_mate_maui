using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using Font = Microsoft.Maui.Font;
using CommunityToolkit.Maui.Core;
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

            // Toon een loading-indicator
            await Navigation.PushModalAsync(new LoadingPageCreateCargoFromPdf(), true);

            // Verwerk het bestand via de ViewModel
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
            // Verwijder de loading-indicator, ongeacht succes of fout
            await Navigation.PopModalAsync(true);
        }
    }


    private async void ScanCargoDocument_OnClicked(object sender, EventArgs e)
    {
        try
        {
            // Activeer de camera om het document te scannen
            Stream documentStream = await CaptureDocumentFromCameraAsync();

            if (documentStream == null)
            {
                await _uiService.ShowSnackbarWarning("No document scanned.");
                return;
            }

            // Toon een loading-indicator
            await Navigation.PushModalAsync(new LoadingPageCreateCargoFromPdf(), true);

            // Verwerk het gescande document via de ViewModel
            bool isCreated = await _cargoCreateViewModel.UploadAndProcessPdfAsync(documentStream);

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
            await _uiService.ShowSnackbarWarning($"An error occurred: {ex.Message}");
        }
        finally
        {
            // Verwijder de loading-indicator, ongeacht succes of fout
            await Navigation.PopModalAsync(true);
        }
    
    }
    
    public async Task<Stream> CaptureDocumentFromCameraAsync()
    {
        var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
        {
            Title = "Please scan the document"
        });

        if (photo != null)
        {
            var stream = await photo.OpenReadAsync();
            return stream;
        }
        return null;
    }
}