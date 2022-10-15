using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Common.ClimateRanges;

[ClimateRangeFor(PetSpecies.PolarBear)]
internal struct PolarBearClimateRange : IClimateRange
{
    public int TempMinC => -60;
    public int TempMaxC => 15;
    public bool IsAquatic => false;
}