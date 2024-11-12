namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface ITextToSpeechService{

    Task SynthesizeSpeechAsync(string text);
}