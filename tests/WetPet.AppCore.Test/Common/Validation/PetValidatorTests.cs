using AutoFixture.Xunit2;
using WetPet.AppCore.Common.Validation;
using WetPet.AppCore.Entities;

namespace WetPet.AppCore.Test.Common.Validation;

public class PetValidatorTests
{
    private readonly PetValidator _sut;

    public PetValidatorTests()
    {
        _sut = new PetValidator();
    }

    [Theory, AutoData]
    public void ValidPet(Pet pet)
    {
        var result = _sut.Validate(pet);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void InvalidPet()
    {
        var pet = new Pet { Name = " ", Location = new() { City = "Berlin", State = " " } };
        var result = _sut.Validate(pet);

        Assert.False(result.IsValid);
        Assert.Contains("Name", result.Errors.Select(x => x.PropertyName));
        Assert.Contains("Location.State", result.Errors.Select(x => x.PropertyName));
    }
}