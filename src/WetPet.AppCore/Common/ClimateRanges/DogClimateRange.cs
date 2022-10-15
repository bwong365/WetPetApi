using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Common.ClimateRanges;

[ClimateRangeFor(PetSpecies.Dog)]
internal struct DogClimateRange : IClimateRange
{
    public int TempMinC => -25;
    public int TempMaxC => 28;
    public bool IsAquatic => false;
}