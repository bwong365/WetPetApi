namespace WetPetAPI.WetPet.Infrastructure.Http.OpenWeatherMap;

public class OpenWeatherMapSettings
{
    public const string SectionName = "OpenWeatherMapSettings";
    public string ApiKey { get; set; } = null!;
    public string GeoBaseUrl { get; set; } = null!;
    public string WeatherBaseUrl { get; set; } = null!;
}