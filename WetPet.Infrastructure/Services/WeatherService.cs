using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.ValueObjects;
using WetPet.Infrastructure.Http.OpenWeatherMap;

namespace WetPet.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly IOpenWeatherMapHttpService _httpService;
    private readonly ILocationService _locationService;
    private readonly IMemoryCache _cache;

    public WeatherService(IOpenWeatherMapHttpService httpService, ILocationService locationService, IMemoryCache cache)
    {
        _httpService = httpService;
        _locationService = locationService;
        _cache = cache;
    }

    public async Task<ErrorOr<WeatherData>> GetWeatherDataAsync(Location location, CancellationToken? ct)
    {
        var cacheKey = $"{location.City},{location.State},{location.Country}";
        _cache.TryGetValue(cacheKey, out WeatherData? cachedData);
        if (cachedData is not null)
        {
            return cachedData;
        }

        var coordinates = await _locationService.GetCoordinatesAsync(location, ct);
        if (coordinates.IsError)
        {
            return coordinates.Errors;
        }

        var weatherResponse = await _httpService.GetWeatherDataAsync(coordinates.Value!, ct);
        if (weatherResponse.IsError)
        {
            return weatherResponse.Errors;
        }

        var weatherData = new WeatherData
        {
            TempC = (int) Math.Round(weatherResponse.Value!.Main.Temp, MidpointRounding.AwayFromZero),
            Condition = weatherResponse.Value.Weather[0]?.Main switch
            {
                MainCondition.Drizzle or MainCondition.Rain => WeatherCondition.Rain,
                MainCondition.Thunderstorm or MainCondition.Tornado => WeatherCondition.Storm,
                MainCondition.Snow => WeatherCondition.Snow,
                _ => WeatherCondition.Normal
            }
        };

        _cache.Set(cacheKey, weatherData, TimeSpan.FromMinutes(30));
        return weatherData;
    }
}