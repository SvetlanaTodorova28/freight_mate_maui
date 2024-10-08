namespace Mde.Project.Mobile.WebAPI.Services.Interfaces;

public interface IEnumService{
    Task<IEnumerable<string>> GetAccessLevelTypeAsync();
}