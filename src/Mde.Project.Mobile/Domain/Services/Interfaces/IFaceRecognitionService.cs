namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IFaceRecognitionService{
    Task<bool> AuthenticateAsync(string reason);
}