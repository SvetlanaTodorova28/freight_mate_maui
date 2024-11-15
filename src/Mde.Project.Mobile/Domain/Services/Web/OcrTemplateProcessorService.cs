using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class OcrTemplateProcessorService : IOcrTemplateProcessorService
{
    private readonly IOcrService _ocrService;

    public OcrTemplateProcessorService(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    public async Task<Cargo> ProcessPdfAsync(Stream pdfStream)
    {
        string extractedText = await _ocrService.ExtractTextFromPdfAsync(pdfStream);
        return ParseExtractedTextToCargo(extractedText);
    }

    private Cargo ParseExtractedTextToCargo(string text)
    {
        // Logica om de geëxtraheerde tekst te parsen en om te zetten naar een Cargo object
        var cargo = new Cargo();
        // Vul de eigenschappen van Cargo op basis van de geëxtraheerde tekst
        return cargo;
    }
}
