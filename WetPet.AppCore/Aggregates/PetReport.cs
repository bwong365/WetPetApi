using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Entities;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Aggregates;

public class PetReport
{
    public Pet Pet { get; set; } = null!;
    public List<PetStatus> Statuses { get; set; } = new();
    public WeatherData WeatherData { get; set; } = null!;
}