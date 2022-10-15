using Moq;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.Services;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Test.Services;

public class PetStatusServiceTests
{
    private readonly Mock<IClimateRangeProvider> _climateRangeProvider;
    private readonly PetStatusService _sut;

    public PetStatusServiceTests()
    {
        _climateRangeProvider = new Mock<IClimateRangeProvider>();
        _sut = new PetStatusService(_climateRangeProvider.Object);
    }

    [Theory]
    [MemberData(nameof(GetStatusesTestData))]
    public void Returns_CorrectStatuses(List<PetStatus> expectedStatuses, IClimateRange climateRange, WeatherData weatherData)
    {
        _climateRangeProvider.Setup(p => p.GetClimateRange(It.IsAny<PetSpecies>())).Returns(ErrorOr.ErrorOr.From(climateRange));

        var result = _sut.GetPetStatuses(PetSpecies.Dog, weatherData).Value.ToList();

        Assert.Equal(expectedStatuses.OrderBy(x => x), result.OrderBy(x => x));
    }

    private class FakeClimateRange : IClimateRange
    {

        public int TempMinC { get; init; }
        public int TempMaxC { get; init; }
        public bool IsAquatic { get; init; }

    }

    private static IEnumerable<object[]> GetStatusesTestData()
    {
        yield return new object[]
        {
            new List<PetStatus> { PetStatus.Content },
            new FakeClimateRange { TempMinC = 0, TempMaxC = 10, IsAquatic = false },
            new WeatherData { TempC = 5, Condition = WeatherCondition.Normal }
        };

        yield return new object[]
        {
            new List<PetStatus> { PetStatus.Hot, PetStatus.Wet },
            new FakeClimateRange { TempMinC = -50, TempMaxC = -10, IsAquatic = false },
            new WeatherData { TempC = 12, Condition = WeatherCondition.Rain }
        };

        yield return new object[]
        {
            new List<PetStatus> { PetStatus.Cold, PetStatus.Snowman },
            new FakeClimateRange { TempMinC = 0, TempMaxC = 10, IsAquatic = false },
            new WeatherData { TempC = -12, Condition = WeatherCondition.Snow }
        };

        yield return new object[]
        {
            new List<PetStatus> { PetStatus.Cold, PetStatus.Dry, PetStatus.Scared },
            new FakeClimateRange { TempMinC = 0, TempMaxC = 10, IsAquatic = true },
            new WeatherData { TempC = -12, Condition = WeatherCondition.Storm }
        };

        yield return new object[]
        {
            new List<PetStatus> { PetStatus.Scared },
            new FakeClimateRange { TempMinC = 0, TempMaxC = 10, IsAquatic = false },
            new WeatherData { TempC = 5, Condition = WeatherCondition.Storm }
        };
    }
}