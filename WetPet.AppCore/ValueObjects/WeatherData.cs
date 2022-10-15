using WetPet.AppCore.Common.Enums;

namespace WetPet.AppCore.ValueObjects;

public record WeatherData
{
    public int TempC { get; init; }
    public WeatherCondition Condition { get; init; }
}