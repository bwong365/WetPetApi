using WetPet.AppCore.Common.Enums;

namespace WetPet.AppCore.Common.ClimateRanges;

internal sealed class ClimateRangeForAttribute : Attribute
{
    public ClimateRangeForAttribute(PetSpecies species)
    {
        Species = species;
    }

    public PetSpecies Species { get; }
}