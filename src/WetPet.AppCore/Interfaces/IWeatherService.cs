using ErrorOr;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Interfaces;

public interface IWeatherService
{
    Task<ErrorOr<WeatherData>> GetWeatherDataAsync(Location location, CancellationToken? cancellationToken = default);
}