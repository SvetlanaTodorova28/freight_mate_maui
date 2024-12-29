namespace Mde.Project.Mobile.Domain.Services.Interfaces;

public interface IGeocodingService{
    Task<IEnumerable<Location>> GetLocationsAsync(string address);
}