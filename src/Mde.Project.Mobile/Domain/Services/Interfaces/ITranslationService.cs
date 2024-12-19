using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ITranslationService{
    Task<ServiceResult<string>> TranslateTextAsync(string text, string targetLanguageCode);
}