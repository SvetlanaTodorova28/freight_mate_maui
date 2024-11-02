namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IAuthFaceRecognition{
     public bool IsSupported();
     Task<NativeAuthResult> PromptLoginAsync(string prompt);
}