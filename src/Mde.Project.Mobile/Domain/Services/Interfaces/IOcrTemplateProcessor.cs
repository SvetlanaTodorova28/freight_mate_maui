
using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IOcrTemplateProcessorService{
    Task<Cargo> ProcessPdfAsync(Stream pdfStream);
}