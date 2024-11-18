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

    public async Task<string> ProcessPdfAsync(Stream pdfStream)
    {
        return await _ocrService.ExtractTextFromPdfAsync(pdfStream);
    }


  
}
