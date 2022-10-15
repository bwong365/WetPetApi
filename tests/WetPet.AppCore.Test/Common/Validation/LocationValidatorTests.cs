using AutoFixture.Xunit2;
using WetPet.AppCore.Common.Validation;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Test.Common.Validation;

public class LocationValidatorTests
{
    private readonly LocationValidator _sut;

    public LocationValidatorTests()
    {
        _sut = new LocationValidator();
    }

    [Theory, AutoData]
    public void ValidLocation(Location location)
    {
        var result = _sut.Validate(location);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void InvalidLocation()
    {
        var location = new Location { City = " ", State = " " };
        var result = _sut.Validate(location);

        Assert.False(result.IsValid);
        Assert.Contains("City", result.Errors.Select(x => x.PropertyName));
        Assert.Contains("Country", result.Errors.Select(x => x.PropertyName));
    }
}