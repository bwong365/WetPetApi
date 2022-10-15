using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Common.ClimateRanges;


[ClimateRangeFor(PetSpecies.Camel)]
internal struct CamelClimateRange : IClimateRange
{
    public int TempMinC => 10;
    public int TempMaxC => 50;
    public bool IsAquatic => false;
}