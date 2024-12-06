using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class TranslationStorageService : ITranslationStorageService
{
    private string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Translations");

    public async Task SaveTranslationAsync(TranslationSpeechModel model)
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string translationsFolder = Path.Combine(folderPath, "Translations");
        Directory.CreateDirectory(translationsFolder); 

        string fileName = $"Translation_{DateTime.UtcNow.Ticks}.json";
        var filePath = Path.Combine(folderPath, fileName);
        var json = JsonConvert.SerializeObject(model);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<List<TranslationSpeechModel>> LoadTranslationsAsync()
    {
        var models = new List<TranslationSpeechModel>();
        foreach (var file in Directory.EnumerateFiles(folderPath, "*.json"))
        {
            var json = await File.ReadAllTextAsync(file);
            var model = JsonConvert.DeserializeObject<TranslationSpeechModel>(json);
            models.Add(model);
        }
        return models;
    }
}