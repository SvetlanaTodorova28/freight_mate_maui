namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IAuthFaceRecognition{
     SupportStatus IsSupported();
     Task<NativeAuthResult> PromptLoginAsync(string prompt);
}