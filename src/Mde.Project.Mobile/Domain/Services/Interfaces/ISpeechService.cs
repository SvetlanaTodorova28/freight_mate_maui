namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ISpeechService{
    Task<string> RecognizeSpeechAsync();
    void SetRecognitionLanguage(string languageCode);
}