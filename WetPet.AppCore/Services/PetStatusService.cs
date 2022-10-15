using ErrorOr;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Services;

public class PetStatusService : IPetStatusService
{
    private readonly IClimateRangeProvider _climateRangeProvider;

    public PetStatusService(IClimateRangeProvider climateRangeProvider)
    {
        _climateRangeProvider = climateRangeProvider;
    }

    public ErrorOr<List<PetStatus>> GetPetStatuses(PetSpecies species, WeatherData weatherData)
    {
        var response = _climateRangeProvider.GetClimateRange(species);

        if (response.IsError)
        {
            return response.Errors;
        }

        var climateRange = response.Value;

        var rainStatus = GetRainStatus(weatherData.Condition, climateRange.IsAquatic);
        var temperatureStatus = GetTemperatureStatus(weatherData.TempC, climateRange);
        var otherStatus = GetOtherStatus(weatherData.Condition);

        var statuses = new List<PetStatus?> { rainStatus, temperatureStatus, otherStatus }
            .Where(s => s is not null)
            .Select(s => s!.Value)
            .ToList();

        if (statuses.Count == 0)
        {
            statuses.Add(PetStatus.Content);
        }

        return statuses;
    }

    private PetStatus? GetRainStatus(WeatherCondition condition, bool isAquatic)
    {
        var isRainy = condition == WeatherCondition.Rain;
        return (isAquatic, isRainy) switch
        {
            (false, true) => PetStatus.Wet,
            (true, false) => PetStatus.Dry,
            _ => null
        };
    }

    private PetStatus? GetTemperatureStatus(int currentTemperature, IClimateRange climateRange)
    {
        return currentTemperature switch
        {
            var currentTemp when currentTemp < climateRange.TempMinC => PetStatus.Cold,
            var currentTemp when currentTemp > climateRange.TempMaxC => PetStatus.Hot,
            _ => null
        };
    }

    private PetStatus? GetOtherStatus(WeatherCondition condition)
    {
        return condition switch
        {
            WeatherCondition.Snow => PetStatus.Snowman,
            WeatherCondition.Storm => PetStatus.Scared,
            _ => null
        };
    }
}