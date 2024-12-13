using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IOcrService{
    Task<ServiceResult<string>> ExtractTextFromPdfAsync(Stream pdfStream);
    Task<ServiceResult<string>> ExtractTextFromImageAsync(Stream imageStream);
    
}