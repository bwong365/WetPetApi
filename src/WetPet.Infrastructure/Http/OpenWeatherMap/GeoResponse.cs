namespace WetPet.Infrastructure.Http.OpenWeatherMap;

public class GeoResponse
{
    public string Name { get; set; } = null!;
    public LocalNames LocalNames { get; set; } = new();
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Country { get; set; } = null!;
    public string State { get; set; } = null!;

}

public class LocalNames
{
    public string En { get; set; } = null!;
    public string Ru { get; set; } = null!;
    public string Uk { get; set; } = null!;
    public string Zh { get; set; } = null!;
}

