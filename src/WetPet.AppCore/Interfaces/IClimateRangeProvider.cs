using ErrorOr;
using WetPet.AppCore.Common.Enums;

namespace WetPet.AppCore.Interfaces;

public interface IClimateRangeProvider
{
    public ErrorOr<IClimateRange> GetClimateRange(PetSpecies species);
}