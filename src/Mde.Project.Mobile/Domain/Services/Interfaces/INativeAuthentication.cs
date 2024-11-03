namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface INativeAuthentication{
     SupportStatus IsSupported();
     Task<NativeAuthResult> PromptLoginAsync(string prompt);
     
}