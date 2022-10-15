namespace WetPet.AppCore.ValueObjects;

public record Coordinates
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}