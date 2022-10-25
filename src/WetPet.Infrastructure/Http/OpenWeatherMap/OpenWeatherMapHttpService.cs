using System.Text.Json;
using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.ValueObjects;
using WetPet.Infrastructure.Common.Serialization;
using WetPetAPI.WetPet.Infrastructure.Http.OpenWeatherMap;

namespace WetPet.Infrastructure.Http.OpenWeatherMap;

public class OpenWeatherMapHttpService : IOpenWeatherMapHttpService
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherMapSettings _settings;
    private readonly IMemoryCache _cache;
    public OpenWeatherMapHttpService(HttpClient httpClient, IOptions<OpenWeatherMapSettings> options, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _cache = cache;
    }

    public async Task<ErrorOr<GeoResponse[]?>> GetLocationDataAsync(Location location, CancellationToken? ct)
    {
        var url = $"{_settings.GeoBaseUrl}?q={location.City},{location.State ?? ""},{location.Country}&appid={_settings.ApiKey}";
        _cache.TryGetValue(url, out GeoResponse[]? cachedData);
        if (cachedData is not null)
        {
            return cachedData;
        }

        var httpResponse = await _httpClient.GetAsync(url, ct ?? default);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return Errors.Location.Failure;
        }
        var geoResponse = await DeserializeAsync<GeoResponse[]>(httpResponse, ct);

        _cache.Set(url, geoResponse, TimeSpan.FromMinutes(1440));
        return geoResponse;
    }

    public async Task<ErrorOr<WeatherResponse?>> GetWeatherDataAsync(Coordinates coordinates, CancellationToken? ct)
    {
        var url = $"{_settings.WeatherBaseUrl}?lat={coordinates.Latitude}&lon={coordinates.Longitude}&units=metric&appid={_settings.ApiKey}";
        _cache.TryGetValue(url, out WeatherResponse? cachedData);
        if (cachedData is not null)
        {
            return cachedData;
        }

        var httpResponse = await _httpClient.GetAsync(url, ct ?? default);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return Errors.Weather.Failure;
        }
        var weatherResponse = await DeserializeAsync<WeatherResponse>(httpResponse, ct);

        _cache.Set(url, weatherResponse, TimeSpan.FromMinutes(10));
        return weatherResponse;
    }

    private async Task<T?> DeserializeAsync<T>(HttpResponseMessage httpResponse, CancellationToken? ct)
    {
        var contentStream = await httpResponse.Content.ReadAsStreamAsync(ct ?? default);
        return await JsonSerializer.DeserializeAsync<T>(
            contentStream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            },
            ct ?? default);
    }
}
