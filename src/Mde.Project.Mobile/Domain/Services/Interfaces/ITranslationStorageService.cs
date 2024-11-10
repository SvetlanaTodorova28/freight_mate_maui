using Mde.Project.Mobile.Domain.Models;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ITranslationStorageService{
    Task SaveTranslationAsync(TranslationSpeechModel model);
    Task<List<TranslationSpeechModel>> LoadTranslationsAsync();
}