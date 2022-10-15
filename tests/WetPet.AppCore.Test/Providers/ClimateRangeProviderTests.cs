using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Providers;

namespace WetPet.AppCore.Test.Providers;

public class ClimateRangeProviderTests
{
    [Theory]
    [InlineData(PetSpecies.Camel, false)]
    [InlineData(PetSpecies.Swordfish, true)]
    public void ReturnsCorrect_ClimateRange(PetSpecies species, bool isAquatic)
    {
        var climateRangeProvider = new ClimateRangeProvider();
        var result = climateRangeProvider.GetClimateRange(species);
        Assert.Equal(result.Value.IsAquatic, isAquatic);
    }
}