using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ISpeechService{
    
    Task<ServiceResult<bool>> SetRecognitionLanguageAsync(string languageCode);
    Task<ServiceResult<bool>> StartContinuousRecognitionAsync(Action<string> onRecognized, Action<string> onError, Action<string> onInfo);
    Task<ServiceResult<bool>> StopContinuousRecognitionAsync();
}