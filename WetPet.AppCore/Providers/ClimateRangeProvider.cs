using System.Reflection;
using ErrorOr;
using WetPet.AppCore.Common.ClimateRanges;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.Common.Errors;

namespace WetPet.AppCore.Providers;

public class ClimateRangeProvider : IClimateRangeProvider
{
    private readonly Dictionary<PetSpecies, IClimateRange> _climateRangeMap;

    public ClimateRangeProvider()
    {
        _climateRangeMap = ScanClimateRangesToMap();
    }

    public ErrorOr<IClimateRange> GetClimateRange(PetSpecies species)
    {
        var range = _climateRangeMap[species];
        if (range is null)
        {
            return Errors.Pet.UnknownSpecies;
        }
        return ErrorOr.ErrorOr.From(range);
    }

    private Dictionary<PetSpecies, IClimateRange> ScanClimateRangesToMap()
    {
        var types = typeof(ClimateRangeProvider).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract);
        return typeof(ClimateRangeProvider).Assembly.GetTypes()
            .Where(t => !t.IsInterface
                && t.IsAssignableTo(typeof(IClimateRange))
                && t.GetCustomAttribute<ClimateRangeForAttribute>() is not null)
            .ToDictionary(t => t.GetCustomAttribute<ClimateRangeForAttribute>()!.Species, t => (IClimateRange) Activator.CreateInstance(t)!);
    }
}