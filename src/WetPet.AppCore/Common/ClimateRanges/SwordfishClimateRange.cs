using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Common.ClimateRanges;

[ClimateRangeFor(PetSpecies.Swordfish)]
internal struct SwordfishClimateRange : IClimateRange
{
    public int TempMinC => 5;
    public int TempMaxC => 27;
    public bool IsAquatic => true;
}