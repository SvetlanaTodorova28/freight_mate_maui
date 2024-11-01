using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services;

public class FaceRecognitionService:IFaceRecognitionService{
    public Task<bool> AuthenticateAsync(string reason){
        throw new NotImplementedException();
    }
}