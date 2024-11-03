using Mde.Project.Mobile.Domain.Services;
using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Platforms;

public class FaceRecognitionService:IAuthFaceRecognition{
    public SupportStatus IsSupported(){
        throw new NotImplementedException();
    }

    public Task<NativeAuthResult> PromptLoginAsync(string prompt){
        throw new NotImplementedException();
    }
}