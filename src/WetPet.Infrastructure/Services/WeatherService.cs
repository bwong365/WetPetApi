using ErrorOr;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.ValueObjects;
using WetPet.Infrastructure.Http.OpenWeatherMap;

namespace WetPet.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly IOpenWeatherMapHttpService _httpService;
    private readonly ILocationService _locationService;

    public WeatherService(IOpenWeatherMapHttpService httpService, ILocationService locationService)
    {
        _httpService = httpService;
        _locationService = locationService;
    }

    public async Task<ErrorOr<WeatherData>> GetWeatherDataAsync(Location location, CancellationToken? ct)
    {
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

        return weatherData;
    }
}