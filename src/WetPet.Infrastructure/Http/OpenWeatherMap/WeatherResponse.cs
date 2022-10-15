using System.Text.Json.Serialization;

namespace WetPet.Infrastructure.Http.OpenWeatherMap;

public partial class WeatherResponse
{
    public Coord Coord { get; set; } = null!;
    public Weather[] Weather { get; set; } = Array.Empty<Weather>();
    public string Base { get; set; } = null!;
    public Main Main { get; set; } = null!;
    public long Visibility { get; set; }
    public Wind Wind { get; set; } = null!;
    public Clouds Clouds { get; set; } = null!;
    public long Dt { get; set; }
    public Sys Sys { get; set; } = null!;
    public long Timezone { get; set; }
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long Cod { get; set; }
}

public partial class Clouds
{
    public long All { get; set; }
}

public partial class Coord
{
    public double Lon { get; set; }
    public double Lat { get; set; }
}

public partial class Main
{
    public double Temp { get; set; }
    public double FeelsLike { get; set; }
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public long Pressure { get; set; }
    public long Humidity { get; set; }
}

public partial class Sys
{
    public long Type { get; set; }
    public long Id { get; set; }
    public string Country { get; set; } = null!;
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
}

public partial class Weather
{
    public long Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MainCondition Main { get; set; }
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = null!;
}

public partial class Wind
{
    public double Speed { get; set; }
    public long Deg { get; set; }
}

public enum MainCondition
{
    Thunderstorm,
    Drizzle,
    Rain,
    Snow,
    Mist,
    Smoke,
    Haze,
    Dust,
    Fog,
    Sand,
    Ash,
    Squall,
    Tornado,
    Clear,
    Clouds
}