using Mde.Project.Mobile.Domain.Services.Interfaces;

namespace Mde.Project.Mobile.Domain.Services.Web;

public class GeocodingService :IGeocodingService{
    public async Task<IEnumerable<Location>> GetLocationsAsync(string address)
    {
        return await Geocoding.GetLocationsAsync(address);
    }

    public async Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location){
       return await Geocoding.GetPlacemarksAsync(location);
    }
}