using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ITranslationStorageService{
    Task<ServiceResult<bool>> SaveTranslationAsync(TranslationSpeechModel model);
    Task<ServiceResult<List<TranslationSpeechModel>>> LoadTranslationsAsync();
}