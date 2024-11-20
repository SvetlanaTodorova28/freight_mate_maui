namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IOcrService{
    Task<string> ExtractTextFromPdfAsync(Stream fileStream);
    Task<string> ExtractTextFromImageAsync(Stream fileStream);
}