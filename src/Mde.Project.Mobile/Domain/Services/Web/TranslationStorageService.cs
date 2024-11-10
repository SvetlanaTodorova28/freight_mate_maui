using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class TranslationStorageService:ITranslationStorageService{
    private string _folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public async Task SaveTranslationAsync(TranslationSpeechModel model)
    {
        string fileName = $"Translation_{DateTime.UtcNow.Ticks}.json";
        string filePath = Path.Combine(_folderPath, fileName);

        string json = JsonConvert.SerializeObject(model);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<List<TranslationSpeechModel>> LoadTranslationsAsync()
    {
        var models = new List<TranslationSpeechModel>();
        foreach (var file in Directory.EnumerateFiles(_folderPath, "Translation_*.json"))
        {
            string json = await File.ReadAllTextAsync(file);
            models.Add(JsonConvert.DeserializeObject<TranslationSpeechModel>(json));
        }
        return models;
    }
}