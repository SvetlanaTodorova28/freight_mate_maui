namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ITranslationService{
    Task<string> TranslateTextAsync(string text, string targetLanguageCode);
}