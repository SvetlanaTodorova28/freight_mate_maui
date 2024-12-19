using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Newtonsoft.Json;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class TranslationStorageService : ITranslationStorageService
{
    private readonly string _folderPath;

    public TranslationStorageService()
    {
        _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Translations");
    }

    public async Task<ServiceResult<bool>> SaveTranslationAsync(TranslationSpeechModel model)
    {
        try
        {
            Directory.CreateDirectory(_folderPath); 

            string fileName = $"Translation_{DateTime.UtcNow.Ticks}.json";
            var filePath = Path.Combine(_folderPath, fileName);

            var json = JsonConvert.SerializeObject(model);
            await File.WriteAllTextAsync(filePath, json);

            return ServiceResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Failure($"Failed to save translation: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<TranslationSpeechModel>>> LoadTranslationsAsync()
    {
        try
        {
            if (!Directory.Exists(_folderPath))
            {
                return ServiceResult<List<TranslationSpeechModel>>.Success(new List<TranslationSpeechModel>());
            }

            var models = new List<TranslationSpeechModel>();

            foreach (var file in Directory.EnumerateFiles(_folderPath, "*.json"))
            {
                var json = await File.ReadAllTextAsync(file);
                var model = JsonConvert.DeserializeObject<TranslationSpeechModel>(json);
                if (model != null)
                {
                    models.Add(model);
                }
            }

            return ServiceResult<List<TranslationSpeechModel>>.Success(models);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<TranslationSpeechModel>>.Failure($"Failed to load translations: {ex.Message}");
        }
    }
}
