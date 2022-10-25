using ErrorOr;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.ValueObjects;
using WetPet.Infrastructure.Http.OpenWeatherMap;

namespace WetPet.Infrastructure.Services;

public class LocationService : ILocationService
{
    private readonly IOpenWeatherMapHttpService _openWeatherMapHttpService;

    public LocationService(IOpenWeatherMapHttpService openWeatherMapHttpService)
    {
        _openWeatherMapHttpService = openWeatherMapHttpService;
    }

    public async Task<ErrorOr<Coordinates?>> GetCoordinatesAsync(Location location, CancellationToken? ct)
    {
        var locationData = await _openWeatherMapHttpService.GetLocationDataAsync(location, ct);
        if (locationData.IsError)
        {
            return locationData.Errors;
        }

        var geoResponse = locationData.Value!;
        if (geoResponse.Count() == 0)
        {
            return Errors.Location.InvalidLocation;
        }

        var coordinates = new Coordinates { Latitude = geoResponse[0].Lat, Longitude = geoResponse[0].Lon };
        return coordinates;
    }
}