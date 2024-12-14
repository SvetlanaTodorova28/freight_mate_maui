using Mde.Project.Mobile.Domain.Services.Web;

namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ITextToSpeechService{

    Task<ServiceResult<bool>> SynthesizeSpeechAsync(string text);

}