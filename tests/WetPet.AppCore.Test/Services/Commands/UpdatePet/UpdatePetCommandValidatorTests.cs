using AutoFixture.Xunit2;
using WetPet.AppCore.Services.Commands.UpdatePet;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Test.Services.Commands.UpdatePet;

public class UpdatePetCommandValidatorTests
{
    [Theory, AutoData]
    public void Validate_WhenSubIsMissing_ReturnInvalid(string name, Location location)
    {
        var sut = new UpdatePetCommandValidator();

        var result = sut.Validate(new UpdatePetCommand { Name = name, Location = location });

        Assert.False(result.IsValid);
    }

    [Theory, AutoData]
    public void Validate_WhenPetIdIsMissing_ReturnInvalid(string sub, string name, Location location)
    {
        var sut = new UpdatePetCommandValidator();

        var result = sut.Validate(new UpdatePetCommand { Sub = sub, Name = name, Location = location });

        Assert.False(result.IsValid);
    }

    [Theory, AutoData]
    public void Validate_WhenCommandHasOnlySubAndId_ReturnValid(string sub, Guid petId)
    {
        var command = new UpdatePetCommand { Sub = sub, PetId = petId };
        var validator = new UpdatePetCommandValidator();

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Theory, AutoData]
    public void Validate_WhenCommandHasShortName_ReturnInvalid(string sub, Guid petId)
    {
        var command = new UpdatePetCommand { Sub = sub, PetId = petId, Name = "" };
        var validator = new UpdatePetCommandValidator();

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Equal(nameof(UpdatePetCommand.Name), result.Errors.First().PropertyName);
    }

    [Theory, AutoData]
    public void Validate_WhenLocationIsInvalid_ReturnInvalid(string sub, Guid petId)
    {
        var command = new UpdatePetCommand { Sub = sub, PetId = petId, Location = new() { City = "", State = "SK" } };
        var validator = new UpdatePetCommandValidator();

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
    }
}