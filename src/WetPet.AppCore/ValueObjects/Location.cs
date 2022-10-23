namespace WetPet.AppCore.ValueObjects;

public record Location
{
    public string City { get; init; } = null!;
    public string? State { get; init; }
    public string Country { get; init; } = null!;
}