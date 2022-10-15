using ErrorOr;
using WetPet.AppCore.ValueObjects;

namespace WetPet.Infrastructure.Http.OpenWeatherMap;


public interface IOpenWeatherMapHttpService
{
    Task<ErrorOr<GeoResponse[]?>> GetLocationDataAsync(Location location, CancellationToken? cancellationToken = default);
    Task<ErrorOr<WeatherResponse?>> GetWeatherDataAsync(Coordinates coordinates, CancellationToken? cancellationToken = default);
}