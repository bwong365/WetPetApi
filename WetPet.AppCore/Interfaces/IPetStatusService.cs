using ErrorOr;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Interfaces;

public interface IPetStatusService
{
    public ErrorOr<List<PetStatus>> GetPetStatuses(PetSpecies species, WeatherData weatherData);
}