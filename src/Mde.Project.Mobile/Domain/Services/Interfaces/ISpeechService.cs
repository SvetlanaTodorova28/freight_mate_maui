namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ISpeechService{
    Task<string> RecognizeSpeechAsync();
    Task SetRecognitionLanguage(string languageCode);
}